using UnityEngine;
using UnityEngine.InputSystem;

public class TankMovement : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float maxForwardSpeed = 8f;
    //[SerializeField] private float maxReverseSpeed = 4f;
    [SerializeField] private float turnSpeedAtMax = 120f;

    private float p1CurrentSpeed = 0f;
    private float p2CurrentSpeed = 0f;

    [Header("Actions")]
    [SerializeField] private InputActionReference p1Move;
    [SerializeField] private InputActionReference p2Move;

    [Header("Players")]
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;

    //[Header("Movement")]
    //[SerializeField] private float moveSpeed = 5f;
    //[SerializeField] private float rotateSpeed = 120f; // degrees per second
    //[SerializeField] private float accel = 0.5f;
    //[SerializeField] private float currentSpeed = 0;

    private void OnEnable()
    {
        p1Move.action.Enable();
        p2Move.action.Enable();
    }

    private void OnDisable()
    {
        p1Move.action.Disable();
        p2Move.action.Disable();
    }

    private void Update()
    {
        var gamepad1 = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
        var gamepad2 = Gamepad.all.Count > 1 ? Gamepad.all[1] : null;

        // Fall back to action references if no gamepad found
        Vector2 m1 = gamepad1 != null ? gamepad1.leftStick.ReadValue() 
            : p1Move.action.ReadValue<Vector2>();
        Vector2 m2 = gamepad2 != null ? gamepad2.leftStick.ReadValue() 
            : p2Move.action.ReadValue<Vector2>();
        
        HandleTank(p1, m1,ref p1CurrentSpeed);
        HandleTank(p2, m2, ref p2CurrentSpeed);
    }

    private void HandleTank(Transform tank, Vector2 input, ref float currentSpeed)
    {
        if (tank == null) return;

        float inputMagnitude = input.magnitude;

        // If player is pushing the stick
        if (inputMagnitude > 0.1f)
        {
            // Convert stick direction to world direction
            Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;

             // Calculate target rotation
             Quaternion targetRotation = Quaternion.LookRotation(moveDir);

             // Smoothly rotate tank
             tank.rotation = Quaternion.RotateTowards(tank.rotation, targetRotation, turnSpeedAtMax * Time.deltaTime);

            // Accelerate forward
            currentSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            // Decelerate when no input
            if (currentSpeed > 0)
            {
                currentSpeed -= deceleration * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0);
            }
        }

        // Clamp speed
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxForwardSpeed);

        // Move tank forward
        tank.position += tank.forward * currentSpeed * Time.deltaTime;
    }

    //private void HandleTank(Transform tank, Vector2 input, ref float currentSpeed)
    //{
    //    if (tank == null) return;

    //    float forwardInput = input.y;
    //    float turnInput = input.x;

    //    //Acceleration
    //    if (Mathf.Abs(forwardInput) > 0.1f)
    //    {
    //        currentSpeed += forwardInput * acceleration * Time.deltaTime;
    //    }
    //    else
    //    {
    //        //decelerate when there's no input
    //        if (currentSpeed > 0)
    //        {
    //            currentSpeed -= deceleration * Time.deltaTime;
    //            currentSpeed = Mathf.Max(currentSpeed, 0);
    //        }
    //        else if (currentSpeed < 0)
    //        {
    //            currentSpeed += deceleration * Time.deltaTime;
    //            currentSpeed = Mathf.Min(currentSpeed, 0);
    //        }
    //    }

    //    //clamp speeds
    //    currentSpeed = Mathf.Clamp(currentSpeed, -maxReverseSpeed, maxForwardSpeed);

    //    //Movement
    //    tank.position += tank.forward * currentSpeed * Time.deltaTime;

    //    //Steering
    //    if (Mathf.Abs(currentSpeed) > 0.1f)
    //    {
    //        float speedFactor = Mathf.Abs(currentSpeed) / maxForwardSpeed;
    //        float turnAmount = turnInput * turnSpeedAtMax * speedFactor * Time.deltaTime;

    //        //Reverse
    //        if (currentSpeed < 0)
    //            turnAmount *= -1f;

    //        tank.Rotate(0f, turnAmount, 0f);
    //    }
    //    //// X input rotates the tank body, Y input moves forward/back
    //    //float move   = input.y * moveSpeed * Time.deltaTime;
    //    //float rotate = input.x * rotateSpeed * Time.deltaTime;

    //    //tank.Rotate(0f, rotate, 0f);
    //    //tank.position += tank.forward * move;
    //}

    //public void ApplyRecoil(int playerIndex, float recoilAmount)
    //{
    //    if (playerIndex == 1)
    //        p1CurrentSpeed -= recoilAmount;
    //    else if (playerIndex == 2)
    //        p2CurrentSpeed -= recoilAmount;
    //}
}
