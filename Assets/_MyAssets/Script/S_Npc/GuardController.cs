using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
[RequireComponent(typeof(NavMeshAgent))]
public class GuardController : MonoBehaviour
{
    [Header("Perception & Vision")]
    [Tooltip("Distance maximale à laquelle le garde peut voir.")]
    [SerializeField] private float viewDistance = 12f;
    
    [Tooltip("Angle total du cône de vision en degrés (ex: 90° = 45° de chaque côté).")]
    [Range(10f, 180f)]
    [SerializeField] private float viewAngle = 90f;
    
    [Tooltip("Masque des calques occultant la vue (Murs) et incluant la Target (Joueur).")]
    [SerializeField] private LayerMask lineOfSightMask;
    
    [Tooltip("Point d'origine des yeux pour les raycasts de détection.")]
    [SerializeField] private Transform eyesOrigin;

    [Header("Cible & Navigation")]
    [Tooltip("Référence vers le joueur à traquer.")]
    [SerializeField] private Transform target;
    
    [Tooltip("Ensemble ordonné des points de passage pour la ronde.")]
    [SerializeField] private Transform[] patrolWaypoints;
    
    [Tooltip("Distance minimale pour considérer qu'un waypoint est atteint.")]
    [SerializeField] private float waypointTolerance = 1.0f;
    
    [Tooltip("Temps d'attente à chaque waypoint en secondes.")]
    [SerializeField] private float waypointWaitTime = 4.0f;
    private float saveWaypointTime=0f;

    [Header("Paramètres d'Engagement")]
    [Tooltip("Distance en dessous de laquelle le garde s'arrête pour tirer.")]
    [SerializeField] private float attackRange = 5f;
    
    [Tooltip("Délai minimal entre deux tirs successifs.")]
    [SerializeField] private float fireRate = 1.0f;
    
    [Tooltip("Temps pendant lequel le garde cherche la cible après l'avoir perdue de vue.")]
    [SerializeField] private float memoryDuration = 3.0f;

    [Header("Événements de Combat")]
    [Tooltip("Événement déclenché à chaque coup de feu (brancher VFX/SFX/Dégâts).")]
    public UnityEvent onShoot;

    // --- Composants internes ---
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private float lastSeenTime = -999f;
    private Vector3 lastKnownPosition;
    private StateMachine stateMachine;
    private GuardPatrolState patrolState;
    private GuardAlertState alertState;
    private GuardShootingState shootingState;


    // Propriété de lecture publique pour l'outillage de débug (GuardDebugger)
    public string CurrentStateName => stateMachine != null && stateMachine.CurrentState != null 
        ? stateMachine.CurrentState.GetType().Name 
        : "Uninitialized";

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (eyesOrigin == null) eyesOrigin = transform;
        stateMachine = new StateMachine();
        patrolState = new GuardPatrolState(this);
        alertState = new GuardAlertState(this);
        shootingState = new GuardShootingState(this);
        saveWaypointTime = waypointWaitTime;
    }

    private void Start()
    {
        stateMachine.ChangeState(patrolState); 
        
    }

    private void Update()
    {
        stateMachine.Tick();
        if (CanSeeTarget() == true)
        {
            print("oui");
            stateMachine.ChangeState(alertState);
        }
        else if (CanSeeTarget() == false)
        {
            stateMachine.ChangeState(patrolState);
            print("non");
        }
    }

    // =========================================================================
    // API PUBLIQUE POUR LES ÉTATS (MÉTHODES DE CONTEXTE)
    // =========================================================================

    #region Méthodes de Transition (Appelées par les IState)
    public void SwitchToPatrol()   => stateMachine.ChangeState(patrolState);
    public void SwitchToAlert()    => stateMachine.ChangeState(alertState);
    public void SwitchToShooting() => stateMachine.ChangeState(shootingState);

    #endregion

    #region Perception & Vision (Maths 3D)

    
    public bool CanSeeTarget()
    {
        if (target == null) return false;

        Vector3 vectorToTarget = target.position - eyesOrigin.position;
        float distanceToTarget = vectorToTarget.magnitude;

        // 1. Filtre de portée
        if (distanceToTarget > viewDistance) return false;

        // 2. Filtre d'angle (Dot Product)
        Vector3 dirToTarget = vectorToTarget.normalized;
        float cosHalfAngle = Mathf.Cos((viewAngle * 0.5f) * Mathf.Deg2Rad);
        
        // Si le produit scalaire est inférieur au cosinus du demi-angle, la cible est hors du cône
        if (Vector3.Dot(eyesOrigin.forward, dirToTarget) < cosHalfAngle) return false;

        // 3. Filtre d'occlusion (Raycast)
        if (Physics.Raycast(eyesOrigin.position, dirToTarget, out RaycastHit hit, viewDistance, lineOfSightMask))
        {
            // Vérifie si le premier obstacle percuté est bien la cible (ou l'un de ses enfants)
            if (hit.transform == target || hit.transform.IsChildOf(target))
            {
                lastKnownPosition = target.position;
                lastSeenTime = Time.time;
                return true;
            }
        }
        return false;
    }

    public bool IsTargetInAttackRange()
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) <= attackRange;
    }

    public bool HasLostTargetTimeout()
    {
        return Time.time - lastSeenTime > memoryDuration;
    }

    public void FaceTarget()
    {
        if (target == null) return;
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Bloque la rotation sur l'axe horizontal
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);
        }
    }

    public void ExecuteShoot()
    {
        onShoot?.Invoke();
    }

    #endregion

    #region Navigation & Patrouille

    public Transform GetCurrentWaypoint()
    {
        if (patrolWaypoints == null || patrolWaypoints.Length == 0) return null;
        return patrolWaypoints[currentWaypointIndex];
    }

    public void AdvanceToNextWaypoint()
    {
        if (patrolWaypoints == null || patrolWaypoints.Length == 0) return;
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolWaypoints.Length;
    }

    public bool HasReachedCurrentDestination()
    {
        if (!agent.pathPending)
        {
           
            if(waypointWaitTime<=0f)
            {
                if (agent.remainingDistance <= waypointTolerance)
                {
                    waypointWaitTime = saveWaypointTime;
                    //true 
                    return !agent.hasPath || agent.velocity.sqrMagnitude == 0f;
                }
            }
            else
            {
                waypointWaitTime-=Time.deltaTime;
            }
            
        }
        return false;
    }

    public void MoveTo(Vector3 destination)
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.SetDestination(destination);
        }
    }

    public void StopMoving()
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    #endregion

    #region Getters de configuration (Utilisés par les états)

    public Transform Target => target;
    public Vector3 LastKnownPosition => lastKnownPosition;
    public float WaypointWaitTime => waypointWaitTime;
    public float FireRate => fireRate;
    public float ViewDistance => viewDistance;
    public float ViewAngle => viewAngle;

    #endregion

    private void OnDrawGizmosSelected()
    {
        // Gizmos de base dans la scène pour assister le placement des designers
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}