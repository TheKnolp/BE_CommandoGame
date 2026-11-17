using System;
using UnityEditor;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    GameObject _en;
    

    [SerializeField] float fovDegrees;
    [SerializeField] float maxDistance;
    Vector3 flatToTarget;
    Vector3 flatForward;
    bool insideAngle = false;
    bool clearSight = false;
    float cosHalfFov;
    [SerializeField] LayerMask layerMask;

    
    private void Awake()
    {
        cosHalfFov = Mathf.Cos(fovDegrees * 0.5f * Mathf.Deg2Rad);
    }
    void Update()
    {
        Detected();
    }
    private void OnTriggerEnter(Collider other)
    {
        _en = other.gameObject;
    }
    private bool IsDetected(Transform _guard, Transform _target, float _fovDegrees, float _maxDistance)
    {
        //Check 1;
        if (_target == null)
        {
            return false;
        }
        flatToTarget = new Vector3(_en.transform.position.x - this.transform.position.x, 0f, _en.transform.position.z - this.transform.position.z);
        flatForward = new Vector3(this.transform.forward.x, 0f, this.transform.forward.z);

        if (Vector3.Dot(flatForward.normalized, flatToTarget.normalized) >= cosHalfFov)
        {
            insideAngle = true;
        }
        else
        {
            insideAngle = false;
        }
        //IsVisible?
        if (insideAngle)
        {
            RaycastHit hit;
            Vector3 maxDist = _en.transform.position - this.transform.position;
            if (Physics.Raycast(this.transform.position, _en.transform.position - this.transform.position, out hit, maxDist.magnitude, layerMask))
            {
                clearSight = false;
            }
            else
            {
                clearSight = true;
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
        if (IsDetected(this.transform, _en.transform, fovDegrees, maxDistance) == true)
        {
            
            return true;
        }
        else if (IsDetected(this.transform, _en.transform, fovDegrees, maxDistance) == false)
        {
            
            return false;
        }
        else
        {
            return false;
        }
    }
}
