using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerConnectionCheck : MonoBehaviour
{
    public GameObject disconnectedImage;
    public MenuManager menuMan;

    [SerializeField] private bool isPaused = false;

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
                disconnectedImage.SetActive(true);
                TogglePause();
                Debug.Log("CONTROLLER DISCONNECTED!!!!");
            }

            if (change == InputDeviceChange.Reconnected)
            {
                disconnectedImage.SetActive(false);
                TogglePause();
                Debug.Log("CONTROLLER RECONNECTED!!!!");
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        //disconnectedImage.SetActive(isPaused);
        //p2PauseMenu.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
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
