using UnityEngine;
using UnityEngine.InputSystem;

public class TankMovement : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private InputActionReference p1Move;
    [SerializeField] private InputActionReference p2Move;

    [Header("Players")]
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 120f; // degrees per second

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
        
        HandleTank(p1, p1Move.action.ReadValue<Vector2>());
        HandleTank(p2, p2Move.action.ReadValue<Vector2>());
    }

    private void HandleTank(Transform tank, Vector2 input)
    {
        if (tank == null) return;

        // X input rotates the tank body, Y input moves forward/back
        float move   = input.y * moveSpeed * Time.deltaTime;
        float rotate = input.x * rotateSpeed * Time.deltaTime;

        tank.Rotate(0f, rotate, 0f);
        tank.position += tank.forward * move;
    }
}
