using UnityEngine.InputSystem;
using UnityEngine;

public class MovemonetController : MonoBehaviour
{
    private Rigidbody rb;
    private float moveSpeed;
    [SerializeField] private float normalSpeed = 6;
    [SerializeField] private float sprintSpeed = 12;
    private Vector3 moveDirection;
    [SerializeField] private InputActionReference movementAction;

    void Start()
    {
        moveSpeed = normalSpeed;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        moveDirection = movementAction.action.ReadValue<Vector3>();
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : normalSpeed;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed, moveDirection.z * moveSpeed);
    }
}