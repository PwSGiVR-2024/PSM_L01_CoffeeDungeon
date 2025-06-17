using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

public class MovemonetController : MonoBehaviour
{
    private Rigidbody rb;
    private float moveSpeed;
    [SerializeField] private float normalSpeed = 6;
    [SerializeField] private float sprintSpeed = 12;
    private Vector3 moveDirection;

    [Header("References")]
    [SerializeField] private InputActionReference movementsActions;

    void Start()
    {
        moveSpeed = normalSpeed;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        moveDirection = movementsActions.action.ReadValue<Vector3>();
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : normalSpeed;
        }
    }

    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(moveDirection.x, 0, moveDirection.z) * moveSpeed;
        rb.linearVelocity = velocity;

        if (velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
            rb.MoveRotation(smoothedRotation);
        }
    }


}