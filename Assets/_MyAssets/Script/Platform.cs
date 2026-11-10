using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Vector3 _starPosition;
    [SerializeField] Vector3 _targetPosition;
    [SerializeField] float _speed;
    [SerializeField] float _distance;
    private void Awake()
    {
        _starPosition = transform.position;
    }
    void Update()
    {
        float offset = Mathf.PingPong(Time.time *_speed, _distance);
        transform.position = _starPosition + _targetPosition * offset;
    }
}
