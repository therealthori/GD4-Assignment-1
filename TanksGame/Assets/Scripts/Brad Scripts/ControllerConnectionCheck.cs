using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerConnectionCheck : MonoBehaviour
{
    public GameObject warningText;

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
