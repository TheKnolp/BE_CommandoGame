using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class npcPatrol : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    int destPoint = 0;
    private NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        StartCoroutine(stopOnWaypoints());
    }
    IEnumerator stopOnWaypoints()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (waypoints.Length == 0){yield return null;}
            agent.destination = waypoints[destPoint].position;
            destPoint = (destPoint + 1) % waypoints.Length;
            //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, new Quaternion(0f, 0f, MathF.Sin(Time.deltaTime * 100), 0f), 10);
        }
        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(stopOnWaypoints());
    }
}
