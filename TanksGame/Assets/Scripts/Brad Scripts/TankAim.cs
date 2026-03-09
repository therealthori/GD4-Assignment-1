using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TankAim : MonoBehaviour
{
    //[Header("Gamepads")] //when one player disconnects, and then reconnects, the order of the array is changed. doing this so that it keeps the correct tank after reconnecting controller 
    //private Gamepad p1Gamepad;
    //private Gamepad p2Gamepad;

    //public GameObject warningText;

    [Header("Actions")]
    [SerializeField] private InputActionReference p1Aim;
    [SerializeField] private InputActionReference p2Aim;

    [Header("Player Guns")]
    [SerializeField] private Transform p1Gun;
    [SerializeField] private Transform p2Gun;

    [Header("Movement")]
    [SerializeField] private float rotateSpeed = 120f; // degrees per second

    [Header("AIM trajectory line")]
    [SerializeField] private LineRenderer p1Line;
    [SerializeField] private LineRenderer p2Line;

    [SerializeField] private float trajectoryDistance = 15f;
    [SerializeField] private int maxBounces = 1;

    [SerializeField] float scrollSpeed = 2f;

    private void OnEnable()
    {
        p1Aim.action.Enable();
        p2Aim.action.Enable();

        //InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        p1Aim.action.Disable();
        p2Aim.action.Disable();

        //InputSystem.onDeviceChange -= OnDeviceChange;
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

        HandleAim(p1Gun, m1);
        HandleAim(p2Gun, m2);

        DrawTrajectory(p1Gun, p1Line);
        DrawTrajectory(p2Gun, p2Line);

        float offset = Time.time * scrollSpeed;

        p1Line.material.mainTextureOffset = new Vector2(offset, 0);
        p2Line.material.mainTextureOffset = new Vector2(offset, 0);
    }

    private void HandleAim(Transform gun, Vector2 input)
    {
        if (gun == null) return;

        // X input rotates the tank body, Y input moves forward/back
        //float rotate = input.x + input.y * rotateSpeed * Time.deltaTime;
        //float rotate = input.x * rotateSpeed * Time.deltaTime;


        //gun.Rotate(0, rotate, 0f);

        //prevent jitter when stick is near center
        if (input.sqrMagnitude < 0.01f) return;

        //convert stick direction to angle
        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;

        //snap rotation
        gun.rotation = Quaternion.Slerp(gun.rotation, Quaternion.Euler(0f, angle, 0f), rotateSpeed * Time.deltaTime);
    }

    void DrawTrajectory(Transform gun, LineRenderer line)
    {
        if (gun == null || line == null) return;

        Vector3 position = gun.position;
        Vector3 direction = gun.forward;

        line.positionCount = maxBounces + 2;
        line.SetPosition(0, position);

        int index = 1;

        for(int i = 0; i <= maxBounces; i++)
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
}
