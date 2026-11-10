using Unity.Mathematics;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] float _speed=1.0f;
    [SerializeField] Rigidbody _rb;
    private Vector3 _direction;

    CapsuleCollider caps;

    private void Awake()
    {
        if( _rb == null )
        _rb = GetComponent<Rigidbody>();
        caps = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        _direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }
    private void FixedUpdate()
    {
        if(_direction != Vector3.zero )
        {
            Vector3 boundedInput = Vector3.ClampMagnitude(_direction, 1f);
            _rb.linearVelocity = boundedInput * (_speed * Time.fixedDeltaTime);
        }
        else
        {
            _rb.linearVelocity = new Vector3(0,0,0);
        }
        playerView();
    }
    void playerView()
    {
        Vector3 lookTarget = new Vector3();
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,100f, groundMask))
        {
            lookTarget = new Vector3(hit.point.x,transform.position.y, hit.point.z);
        }
        Debug.DrawLine(transform.position, lookTarget);
        transform.LookAt(lookTarget);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider)
        {
           // print(collision.collider.name);
        }
    }
}
