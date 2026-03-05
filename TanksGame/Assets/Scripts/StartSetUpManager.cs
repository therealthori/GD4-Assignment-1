using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StartSetUpManager : MonoBehaviour
{
     [Header("UI")]
    public GameObject startScreen;
    public GameObject player1Image;
    public GameObject player2Image;

    [Header("Scene")]
    public string tutorialScene = "Tutorial";

    private bool player1Ready = false;
    private bool player2Ready = false;
    private bool startScreenHidden = false;

    void Update()
    {
        CheckControllers();

        // ANY controller presses X to hide start screen
        if (!startScreenHidden &&
            (Input.GetKeyDown(KeyCode.Joystick1Button0) ||
             Input.GetKeyDown(KeyCode.Joystick2Button0)))
        {
            startScreen.SetActive(false);
            startScreenHidden = true;
            Debug.Log("Start screen hidden");
        }

        // Player 1 presses Start
        if (!player1Ready && Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            player1Ready = true;
            player1Image.SetActive(false);
            Debug.Log("Player 1 Ready");
        }

        // Player 2 presses Start
        if (!player2Ready && Input.GetKeyDown(KeyCode.Joystick2Button7))
        {
            player2Ready = true;
            player2Image.SetActive(false);
            Debug.Log("Player 2 Ready");
        }

        // Load scene when both players ready
        if (player1Ready && player2Ready && ControllersConnected() >= 2)
        {
            SceneManager.LoadScene(tutorialScene);
        }
    }

    int ControllersConnected()
    {
        string[] controllers = Input.GetJoystickNames();
        int connected = 0;

        foreach (string c in controllers)
        {
            if (!string.IsNullOrEmpty(c))
                connected++;
        }

        return connected;
    }

    void CheckControllers()
    {
        if (ControllersConnected() < 2)
        {
            Debug.LogWarning("Need 2 controllers connected.");
        }
    }
}
