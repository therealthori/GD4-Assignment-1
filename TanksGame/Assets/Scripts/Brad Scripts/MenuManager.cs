using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private InputActionReference p1Pause;
    [SerializeField] private InputActionReference p2Pause;

    [SerializeField] private GameObject p1PauseMenu;
    [SerializeField] private GameObject p2PauseMenu;

    private void OnEnable()
    {
        p1Pause.action.Enable();
        p2Pause.action.Enable();
    }

    private void OnDisable()
    {
        p1Pause.action.Disable();
        p2Pause.action.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var gamepad1 = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
        var gamepad2 = Gamepad.all.Count > 1 ? Gamepad.all[1] : null;

        // Fall back to action references if no gamepad found
        bool m1 = p1Pause.action.ReadValue<bool>();
        bool m2 = p2Pause.action.ReadValue<bool>();

        //HandlePause(p1PauseMenu, m1, ref isPaused);
        //HandlePause(p1PauseMenu, m2, ref isPaused);
    }

    private void HandlePause(GameObject pauseMenu, bool input, bool isPaused)
    {

    }
}
