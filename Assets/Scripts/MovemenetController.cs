using UnityEngine;
using UnityEngine.InputSystem;

public class MovemonetController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float moveSpeed=6;
    private Vector3 moveDirection;
    [SerializeField] private InputActionReference movementAction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveDirection = movementAction.action.ReadValue<Vector3>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity=new Vector3(moveDirection.x*moveSpeed,moveDirection.y*moveSpeed,moveDirection.z*moveSpeed);
    }
}
