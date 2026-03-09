using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private InputActionReference p1Pause;
    [SerializeField] private InputActionReference p2Pause;

    [SerializeField] private bool isPaused = false;

    [SerializeField] private GameObject p1PauseMenu;
    //[SerializeField] private GameObject p2PauseMenu;

    [SerializeField] private GameObject firstSelectedButton;

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
        bool p1Pressed = gamepad1 != null ? gamepad1.startButton.wasPressedThisFrame : p1Pause.action.WasPressedThisFrame();
        bool p2Pressed = gamepad2 != null ? gamepad2.startButton.wasPressedThisFrame : p2Pause.action.WasPressedThisFrame();

        if (p1Pressed || p2Pressed)
        {
            TogglePause();
        }

        //HandlePause(p1PauseMenu, m1, isPaused);
        //HandlePause(p1PauseMenu, m2, isPaused);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        p1PauseMenu.SetActive(isPaused);
        //p2PauseMenu.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit the GAME BOMBOCLAAT");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void TutorialLevel()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
