using System;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Camera _camera;  
    [SerializeField] private float _smoothTime = 1f;
    [SerializeField]Vector3 _offestPosition;
    private Vector3 _viewportPos;
    private Vector3 _position;
    private float _followVelocityX;
    private float _followVelocityY;
    public int framerate;

    bool mouseDown = false;
    private void Awake()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = framerate;
        if (target == null)
        { print("playerNotFound"); }
        if( _camera == null)
            _camera.GetComponent<Camera>();
    }
    void LateUpdate()
    {
        if (target == null)
            return;
        _viewportPos = _camera.WorldToViewportPoint(target.position);
        if(Input.GetMouseButtonDown(1))
        {
            mouseDown = true;
        }
        if(Input.GetMouseButtonUp(1))
        {
            mouseDown = false;
        }

        if(mouseDown==false)
        {
            if (_viewportPos.x >= 0.7f || 0.3f >= _viewportPos.x)
            {
                _position.x = Mathf.SmoothDamp(transform.position.x, target.position.x + _offestPosition.x, ref _followVelocityX, _smoothTime, Mathf.Infinity, Time.deltaTime);
            }
            if (_viewportPos.y >= 0.7f || 0.3f >= _viewportPos.y)
            {
                _position.z = Mathf.SmoothDamp(transform.position.z, target.position.z + _offestPosition.z, ref _followVelocityY, _smoothTime, Mathf.Infinity, Time.deltaTime);
            }
        }
        else if (mouseDown==true)
        {
            //Vector3 lookTarget = new Vector3();
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                _position.x = hit.point.x/4;
                _position.z = hit.point.z/4;
            }
        }
         transform.position = new Vector3(_position.x, _offestPosition.y, _position.z);
    }
}
