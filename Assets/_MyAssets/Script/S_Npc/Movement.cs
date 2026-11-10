using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float _speed=1;
    Transform _guard;
    bool _isAngleMax;
    private void Awake()
    {
        _guard = GetComponent<Transform>();
    }
    void Update()
    {
        _guard.transform.Rotate(Vector3.up, MathF.Sin(Time.deltaTime * _speed)); 
        
        _guard.SetLocalPositionAndRotation(transform.position, transform.rotation);
    }
}
