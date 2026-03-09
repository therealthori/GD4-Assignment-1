using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WinScreenUI : MonoBehaviour
{
    [Header("Winner Images")]
    [SerializeField] private GameObject p1WinImage;  // your P1 wins UI image
    [SerializeField] private GameObject p2WinImage;  // your P2 wins UI image

    [Header("Score Display")]
    [SerializeField] private TextMeshProUGUI p1ScoreText;
    [SerializeField] private TextMeshProUGUI p2ScoreText;

    [Header("Countdown")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float           returnDelay = 5f;

    [Header("Scene")]
    [SerializeField] private string startSceneName = "StartScene";

    private void Start()
    {
        // Hide both images initially
        if (p1WinImage) p1WinImage.SetActive(false);
        if (p2WinImage) p2WinImage.SetActive(false);

        ShowWinner();
        StartCoroutine(CountdownToMenu());
    }

    private void ShowWinner()
    {
        if (ScoreManager.Instance == null) return;

        int p1Score = ScoreManager.Instance.GetPlayerScore(1);
        int p2Score = ScoreManager.Instance.GetPlayerScore(2);
        int winner  = ScoreManager.Instance.GameWinner;

        // Update score labels
        if (p1ScoreText) p1ScoreText.text = $"Player 1:  {p1Score}";
        if (p2ScoreText) p2ScoreText.text = $"Player 2:  {p2Score}";

        // Show correct win image
        switch (winner)
        {
            case 1:
                if (p1WinImage) p1WinImage.SetActive(true);
                break;
            case 2:
                if (p2WinImage) p2WinImage.SetActive(true);
                break;
            default:
                // Tie — show both
                if (p1WinImage) p1WinImage.SetActive(true);
                if (p2WinImage) p2WinImage.SetActive(true);
                break;
        }
    }

    private IEnumerator CountdownToMenu()
    {
        float elapsed = 0f;

        while (elapsed < returnDelay)
        {
            elapsed += Time.deltaTime;

            if (countdownText)
                countdownText.text = $"Returning to menu in {returnDelay - elapsed:F0}s...";

            yield return null;
        }

        ReturnToMenu();
    }

    // Hook this to a "Skip" button if you want
    public void ReturnToMenu()
    {
        StopAllCoroutines();
        ScoreManager.Instance?.ResetScores();
        SceneManager.LoadScene(startSceneName);
    }
}