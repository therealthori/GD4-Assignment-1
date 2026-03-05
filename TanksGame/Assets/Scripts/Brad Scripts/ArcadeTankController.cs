using UnityEngine;
using UnityEngine.InputSystem;

public class ArcadeTankController : MonoBehaviour
{
    private ArcadeTankController controls;
    [SerializeField] private PlayerInput playerInput;
    private InputAction steerAction;

    private Vector2 moveInput;
    //private bool brakePressed;

    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] private Rigidbody tankRB;

    public float acceleration = 500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;

    [SerializeField] private float currentAcceleration = 0f;
    [SerializeField] private float currentBreakForce = 0f;
    [SerializeField] private float currentTurnAngle = 0f;

    //private InputAction moveAction;
    //private InputAction brakeAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        steerAction = playerInput.actions["Steer"];
        //brakeAction = playerInput.actions["Brake"];

        steerAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        steerAction.canceled += ctx => moveInput = Vector2.zero;

        //brakeAction.performed += ctx => brakePressed = true;
        //brakeAction.canceled += ctx => brakePressed = false;
    }
    private void FixedUpdate()
    {
        currentAcceleration = acceleration * moveInput.y;

        if (currentAcceleration == 0)
        {
            tankRB.linearDamping = 0.5f;
            currentBreakForce = breakingForce;
        }
        else
        {
            tankRB.linearDamping = 0f;
        }

        // Brake input
        //if (brakePressed)
        //    currentBreakForce = breakingForce;
        //else
        //    currentBreakForce = 0f;

        // Apply motor torque
        backRight.motorTorque = currentAcceleration;
        backLeft.motorTorque = currentAcceleration;

        // Apply braking
        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;

        // Steering
        currentTurnAngle = maxTurnAngle * moveInput.x;
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
