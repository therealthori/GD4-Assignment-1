using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public TextMeshProUGUI roundWinnerText;
    public TextMeshProUGUI gameWinnerText;
    public GameObject winnerMessagePanel;
    
    void Awake()
    {
        // Singleton pattern for UI
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Subscribe to score events
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoresUpdated += UpdateScoreUI;
            ScoreManager.Instance.OnRoundWinner += ShowRoundWinner;
            ScoreManager.Instance.OnGameWinner += ShowGameWinner;
            
            // Initial UI update
            UpdateScoreUI();
        }
        
        // Hide winner panel at start
        if (winnerMessagePanel != null)
            winnerMessagePanel.SetActive(false);
    }
    
    void UpdateScoreUI()
    {
        if (ScoreManager.Instance == null) return;
        
        if (player1ScoreText != null)
        {
            player1ScoreText.text = "P1: " + ScoreManager.Instance.GetPlayerScore(1);
        }
        
        if (player2ScoreText != null)
        {
            player2ScoreText.text = "P2: " + ScoreManager.Instance.GetPlayerScore(2);
        }
    }
    
    void ShowRoundWinner(int winnerNumber)
    {
        if (roundWinnerText != null)
        {
            StartCoroutine(DisplayWinnerMessage($"Player {winnerNumber} wins the round!", 2f));
        }
    }
    
    void ShowGameWinner(int winnerNumber)
    {
        if (gameWinnerText != null)
        {
            StartCoroutine(DisplayWinnerMessage($"PLAYER {winnerNumber} WINS THE GAME!", 3f));
        }
    }
    
    System.Collections.IEnumerator DisplayWinnerMessage(string message, float duration)
    {
        if (winnerMessagePanel != null)
        {
            // Update the appropriate text based on message type
            if (message.Contains("round"))
                roundWinnerText.text = message;
            else
                gameWinnerText.text = message;
            
            winnerMessagePanel.SetActive(true);
            yield return new WaitForSeconds(duration);
            winnerMessagePanel.SetActive(false);
        }
    }
    
    void OnDestroy()
    {
        // Unsubscribe from events
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoresUpdated -= UpdateScoreUI;
            ScoreManager.Instance.OnRoundWinner -= ShowRoundWinner;
            ScoreManager.Instance.OnGameWinner -= ShowGameWinner;
        }
    }
}
