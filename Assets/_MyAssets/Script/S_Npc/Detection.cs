using System;
using UnityEditor;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField] Transform target;
    Transform guard;

    [SerializeField] float fovDegrees;
    [SerializeField] float maxDistance;
    Vector3 flatToTarget;
    Vector3 flatForward;
    bool withinRange = false;
    bool insideAngle = false;
    bool clearSight = false;
    float cosHalfFov;
    [SerializeField] Vector3[] _pointToCheck;
    bool[] _pointVisible= new bool[4] {false, false, false,false};
    CapsuleCollider targetCapsule;

    [SerializeField]LayerMask layerMask;
    
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetCapsule = target.GetComponent<CapsuleCollider>();
        if (target == null)
        {
            Debug.Log("No Player Find");
            this.enabled = false;
        }

        guard = this.transform;
        cosHalfFov = Mathf.Cos(fovDegrees * 0.5f * Mathf.Deg2Rad);
        _pointToCheck = new Vector3[4] { new Vector3(0, 0, 0), new Vector3(0, 0, targetCapsule.radius), new Vector3(0, 0, -targetCapsule.radius), new Vector3(0, targetCapsule.height/2, 0) };
    }
    void Update()
    {
        Detected();
    }
    private bool IsDetected(Transform _guard,Transform _target,float _fovDegrees,float _maxDistance)
    {
      //Check 1;
      if (_target==null)
      {
          return false;
      }
      flatToTarget = new Vector3(target.position.x-guard.position.x, 0f, target.position.z-guard.position.z);
      flatForward  = new Vector3(guard.forward.x, 0f, guard.forward.z);
      //IsWithinRange?
      if(flatToTarget.sqrMagnitude<=maxDistance*maxDistance)
      {
         withinRange = true;
      }
      else
      {
          withinRange =false; 
      }
      //IsInVision?
      if (withinRange)
        {
            if(Vector3.Dot(flatForward.normalized,flatToTarget.normalized) >=cosHalfFov )
            {
                insideAngle = true;
            }
            else
            {
                insideAngle = false;
            }
        }
      else
        {
            insideAngle = false;
        }
      //IsVisible?
      if(insideAngle)
        {
            RaycastHit hit;
            
            for (int i=1; i<= _pointToCheck.Length-1; i++)
            {
                
                Debug.DrawRay(transform.position, target.transform.position + _pointToCheck[i] - this.transform.position, Color.yellow);

                Vector3 maxDist = target.transform.position + _pointToCheck[i] - this.transform.position;
                if((maxDist.magnitude<=maxDistance))
                {
                    if (Physics.Raycast(this.transform.position, target.transform.position + _pointToCheck[i] - this.transform.position, out hit, maxDist.magnitude, layerMask))
                    {
                        Debug.DrawRay(transform.position, target.transform.position + _pointToCheck[i] - this.transform.position, Color.red);
                        _pointVisible[i] = false;
                    }
                    else
                    {
                        _pointVisible[i] = true;
                        clearSight = true;
                       
                    }
                }
            }
            for (int i = 1; i <= _pointVisible.Length-1; i++)
            {
                if( _pointVisible[i] ==false &&  _pointVisible[i-1] ==false)
                {
                    clearSight=false;
                }
            }
        }
      else
        {
            clearSight = false;
        }

        return clearSight;
    }
    public bool Detected()
    {
        if (IsDetected(guard, target, fovDegrees, maxDistance) == true)
        {
            return true;
        }
        else if (IsDetected(guard, target, fovDegrees, maxDistance) == false)
        {
            return false;
        }
        else
        { 
           return false; 
        }
    }
}
