using UnityEngine;
using UnityEngine.InputSystem;
public class NewMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is create
    [Header("Car Settings")]
    [SerializeField] private float acceleration = 4f;
    [SerializeField] private float deceleration = 4f;
    [SerializeField] private float maxForwardSpeed = 4f;
    [SerializeField] private float turnSpeedAtMax = 200f;

    private float currentSpeed = 0f;

    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    private void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        HandleTank(input);
    }

    private void HandleTank(Vector2 input)
    {
        float inputMagnitude = input.magnitude;

        if (inputMagnitude > 0.1f)
        {
            Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(moveDir);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeedAtMax * Time.deltaTime
            );

            currentSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= deceleration * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0);
            }
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxForwardSpeed);

        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }
}
