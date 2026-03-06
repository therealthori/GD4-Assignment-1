using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TankAim : MonoBehaviour
{
    //[Header("Gamepads")] //when one player disconnects, and then reconnects, the order of the array is changed. doing this so that it keeps the correct tank after reconnecting controller 
    //private Gamepad p1Gamepad;
    //private Gamepad p2Gamepad;

    public GameObject warningText;

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

        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        p1Aim.action.Disable();
        p2Aim.action.Disable();

        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if(device is Gamepad)
        {
            if (change == InputDeviceChange.Disconnected)
            {
                warningText.SetActive(true);
                Debug.Log("CONTROLLER DISCONNECTED!!!!");
            }

            if (change == InputDeviceChange.Reconnected)
            {
                warningText.SetActive(false);
                Debug.Log("CONTROLLER DISCONNECTED!!!!");
            }
        }
    }

    private void Start()
    {
        //NEW NOTE: THIS IS NOT IMPORTANT!!! UNITY INPUT MANAGER WORKS TO RECONNECT PLAYERS. NO CODE NEEDED
        //this isnt the usual way to do it, but it keeps track of which gamepad is assigned [0] first, and second, so the swapping doesnt happen
        //if (Gamepad.all.Count > 0)
            //p1Gamepad = Gamepad.all[0];

        //if (Gamepad.all.Count > 1)
            //p2Gamepad = Gamepad.all[1];
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

        HandleTank(p1Gun, m1);
        HandleTank(p2Gun, m2);
    }

    private void HandleTank(Transform gun, Vector2 input)
    {
        if (gun == null) return;

        // X input rotates the tank body, Y input moves forward/back
        //float rotate = input.x + input.y * rotateSpeed * Time.deltaTime;
        float rotate = input.x * rotateSpeed * Time.deltaTime;


        gun.Rotate(0, rotate, 0f);
    }
}
