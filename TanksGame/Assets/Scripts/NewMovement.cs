using UnityEngine;
using UnityEngine.InputSystem;
public class NewMovement : MonoBehaviour
{
    [Header("Car Settings")]
    public float acceleration = 4f;
    public float deceleration = 4f;
    public float maxForwardSpeed = 4f;
    [SerializeField] private float turnSpeedAtMax = 200f;

    private float currentSpeed = 0f;
    
    [Header("Boost Settings")]
    [SerializeField] private float boostMultiplier = 2f;
    [SerializeField] private float boostDuration = 3f;
    private bool isBoosted = false;
    
    [Header("Boost Effects")]
    [SerializeField] private ParticleSystem boostParticles;

    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private int playerIndex = 0; // Set to 0 for Player 1, 1 for Player 2 in prefab variants

    private void Update()
    {
        Vector2 input = Vector2.zero;
        
        // Try to get input from the appropriate gamepad
        if (Gamepad.all.Count > playerIndex)
        {
            input = Gamepad.all[playerIndex].leftStick.ReadValue();
        }
        // Fallback to keyboard for player 1 (WASD)
        else if (playerIndex == 0 && Keyboard.current != null)
        {
            float horizontal = 0;
            float vertical = 0;
            
            if (Keyboard.current.wKey.isPressed) vertical += 1;
            if (Keyboard.current.sKey.isPressed) vertical -= 1;
            if (Keyboard.current.aKey.isPressed) horizontal -= 1;
            if (Keyboard.current.dKey.isPressed) horizontal += 1;
            
            input = new Vector2(horizontal, vertical);
        }
        // Fallback to arrow keys for player 2
        else if (playerIndex == 1 && Keyboard.current != null)
        {
            float horizontal = 0;
            float vertical = 0;
            
            if (Keyboard.current.upArrowKey.isPressed) vertical += 1;
            if (Keyboard.current.downArrowKey.isPressed) vertical -= 1;
            if (Keyboard.current.leftArrowKey.isPressed) horizontal -= 1;
            if (Keyboard.current.rightArrowKey.isPressed) horizontal += 1;
            
            input = new Vector2(horizontal, vertical);
        }
        
        HandleTank(input);
    }

    private void HandleTank(Vector2 input)
    {
       float inputMagnitude = input.magnitude;

    if (inputMagnitude > 0.1f)
    {
        // Start movement sound if not already playing
        SoundManager.Instance.StartTankMove();

        // Determine movement direction
        Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;

        // Rotate tank towards movement direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            turnSpeedAtMax * Time.deltaTime
        );

        // Accelerate
        currentSpeed += acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxForwardSpeed);

        // Update engine pitch based on current speed
        if (SoundManager.Instance.movementSource != null)
        {
            SoundManager.Instance.movementSource.pitch = 0.8f + (currentSpeed / maxForwardSpeed) * 0.4f;
        }
    }
    else
    {
        // Decelerate when no input
        if (currentSpeed > 0)
        {
            currentSpeed -= deceleration * Time.deltaTime;
            currentSpeed = Mathf.Max(currentSpeed, 0);

            // Update pitch while decelerating
            if (SoundManager.Instance.movementSource != null && currentSpeed > 0)
            {
                SoundManager.Instance.movementSource.pitch = 0.8f + (currentSpeed / maxForwardSpeed) * 0.4f;
            }
        }

        // Stop movement sound when fully stopped
        if (currentSpeed <= 0)
        {
            SoundManager.Instance.StopTankMove();
        }
    }

    // Move the tank forward
    transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }
    
    public void ActivateBoost()
    {
        if (!isBoosted)
        {
            StartCoroutine(BoostRoutine());
        }
    }
    
    private System.Collections.IEnumerator BoostRoutine()
    {
        isBoosted = true;
    
        float originalMaxSpeed = maxForwardSpeed;
        float originalAcceleration = acceleration;
    
        maxForwardSpeed *= boostMultiplier;
        acceleration *= boostMultiplier;
    
        if (boostParticles != null)
            boostParticles.Play();
    
        yield return new WaitForSeconds(boostDuration);
    
        maxForwardSpeed = originalMaxSpeed;
        acceleration = originalAcceleration;
    
        if (boostParticles != null)
            boostParticles.Stop();
    
        isBoosted = false;
    }
}