using UnityEngine;
using UnityEngine.InputSystem;

public class TankAim : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private InputActionReference p1Aim;
    [SerializeField] private InputActionReference p2Aim;

    [Header("Player Guns")]
    [SerializeField] private Transform p1Gun;
    [SerializeField] private Transform p2Gun;

    [Header("Movement")]
    [SerializeField] private float rotateSpeed = 120f; // degrees per second

    private void OnEnable()
    {
        p1Aim.action.Enable();
        p2Aim.action.Enable();
    }

    private void OnDisable()
    {
        p1Aim.action.Disable();
        p2Aim.action.Disable();
    }

    private void Update()
    {
        var gamepad1 = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
        var gamepad2 = Gamepad.all.Count > 1 ? Gamepad.all[1] : null;

        // Fall back to action references if no gamepad found
        Vector2 m1 = gamepad1 != null ? gamepad1.rightStick.ReadValue()
            : p1Aim.action.ReadValue<Vector2>();
        Vector2 m2 = gamepad2 != null ? gamepad2.rightStick.ReadValue()
            : p2Aim.action.ReadValue<Vector2>();

        HandleTank(p1Gun, p1Aim.action.ReadValue<Vector2>());
        HandleTank(p2Gun, p2Aim.action.ReadValue<Vector2>());
    }

    private void HandleTank(Transform gun, Vector2 input)
    {
        if (gun == null) return;

        // X input rotates the tank body, Y input moves forward/back
        float rotate = input.x + input.y * rotateSpeed * Time.deltaTime;
        

        gun.Rotate(0, rotate, 0f);
    }
}
