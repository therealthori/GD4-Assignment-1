using UnityEngine;
using UnityEngine.InputSystem;

public class NewAim : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference aimAction;
    [SerializeField] private int playerIndex = 0; // 0 for Player 1, 1 for Player 2

    [Header("Gun")]
    [SerializeField] private Transform gun;

    [Header("Movement")]
    [SerializeField] private float rotateSpeed = 120f;

    [Header("Trajectory")]
    [SerializeField] private LineRenderer line;
    [SerializeField] private float trajectoryDistance = 15f;
    [SerializeField] private int maxBounces = 1;

    [SerializeField] private float scrollSpeed = 2f;

    // Optional: Store the assigned gamepad for this player
    private Gamepad assignedGamepad;

    private void OnEnable()
    {
        if (aimAction != null)
            aimAction.action.Enable();
    }

    private void OnDisable()
    {
        if (aimAction != null)
            aimAction.action.Disable();
    }

    private void Start()
    {
        // Try to assign gamepad at start
        AssignGamepad();
    }

    private void AssignGamepad()
    {
        // Assign the appropriate gamepad based on player index
        if (Gamepad.all.Count > playerIndex)
        {
            assignedGamepad = Gamepad.all[playerIndex];
            Debug.Log($"Player {playerIndex + 1} assigned to {assignedGamepad.name}");
        }
        else
        {
            assignedGamepad = null;
            Debug.Log($"Player {playerIndex + 1} using keyboard/mouse fallback");
        }
    }

    private void Update()
    {
        // Reassign gamepad if device count changes (player plugged in later)
        if (assignedGamepad == null && Gamepad.all.Count > playerIndex)
        {
            AssignGamepad();
        }

        Vector2 input = GetAimInput();
        HandleAim(input);
        DrawTrajectory();

        // Scroll texture for line renderer
        if (line != null && line.material != null)
        {
            float offset = Time.time * scrollSpeed;
            line.material.mainTextureOffset = new Vector2(offset, 0);
        }
    }

    private Vector2 GetAimInput()
    {
        // Try gamepad first (right stick for aiming)
        if (assignedGamepad != null)
        {
            Vector2 gamepadInput = assignedGamepad.rightStick.ReadValue();
            
            // Only return gamepad input if it's significant
            if (gamepadInput.sqrMagnitude > 0.01f)
            {
                return gamepadInput;
            }
        }

        // Fallback to keyboard/mouse based on player index
        if (playerIndex == 0 && Mouse.current != null)
        {
            // Player 1: Mouse aiming
            return GetMouseAimInput();
        }
        else if (playerIndex == 1 && Keyboard.current != null)
        {
            // Player 2: Arrow keys or IJKL for aiming
            return GetKeyboardAimInput();
        }

        // Final fallback to input action reference
        if (aimAction != null)
        {
            return aimAction.action.ReadValue<Vector2>();
        }

        return Vector2.zero;
    }

    private Vector2 GetMouseAimInput()
    {
        if (gun == null || Camera.main == null) return Vector2.zero;

        // Convert mouse position to world direction
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, gun.position);
        
        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 directionToTarget = (targetPoint - gun.position).normalized;
            
            // Convert world direction to input vector
            return new Vector2(directionToTarget.x, directionToTarget.z);
        }

        return Vector2.zero;
    }

    private Vector2 GetKeyboardAimInput()
    {
        float horizontal = 0;
        float vertical = 0;

        if (Keyboard.current != null)
        {
            // Arrow keys for aiming
            if (Keyboard.current.rightArrowKey.isPressed) horizontal += 1;
            if (Keyboard.current.leftArrowKey.isPressed) horizontal -= 1;
            if (Keyboard.current.upArrowKey.isPressed) vertical += 1;
            if (Keyboard.current.downArrowKey.isPressed) vertical -= 1;

            // Alternative: IJKL keys
            if (Keyboard.current.lKey.isPressed) horizontal += 1;
            if (Keyboard.current.jKey.isPressed) horizontal -= 1;
            if (Keyboard.current.iKey.isPressed) vertical += 1;
            if (Keyboard.current.kKey.isPressed) vertical -= 1;
        }

        return new Vector2(horizontal, vertical).normalized;
    }

    private void HandleAim(Vector2 input)
    {
        if (gun == null) return;

        // Prevent jitter when stick is near center
        if (input.sqrMagnitude < 0.01f) return;

        // Calculate angle from input (x is horizontal, y is vertical/forward)
        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);

        gun.rotation = Quaternion.Slerp(
            gun.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    void DrawTrajectory()
    {
        if (gun == null || line == null) return;

        Vector3 position = gun.position;
        Vector3 direction = gun.forward;

        line.positionCount = maxBounces + 2;
        line.SetPosition(0, position);

        int index = 1;

        for (int i = 0; i <= maxBounces; i++)
        {
            RaycastHit hit;

            if (Physics.Raycast(position, direction, out hit, trajectoryDistance))
            {
                line.SetPosition(index, hit.point);

                direction = Vector3.Reflect(direction, hit.normal);
                position = hit.point;

                index++;
            }
            else
            {
                line.SetPosition(index, position + direction * trajectoryDistance);
                break;
            }
        }
    }

    // Optional: Call this if you want to reassign gamepads (e.g., when player joins late)
    public void SetPlayerIndex(int newPlayerIndex)
    {
        playerIndex = newPlayerIndex;
        AssignGamepad();
    }
}