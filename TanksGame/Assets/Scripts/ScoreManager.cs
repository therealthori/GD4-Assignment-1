using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    [Header("Scores")]
    public int player1Score = 0;
    public int player2Score = 0;

    [Header("UI")]
    public TextMeshProUGUI winText;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerDied(int playerNumber)
    {
        if (playerNumber == 1)
        {
            player2Score++;
            ShowWinner(2);
        }
        else if (playerNumber == 2)
        {
            player1Score++;
            ShowWinner(1);
        }

        UpdateScoreUI();
    }

    void ShowWinner(int winner)
    {
        winText.gameObject.SetActive(true);
        winText.text = "PLAYER " + winner + " WINS!";
    }

    void UpdateScoreUI()
    {
        scoreText.text = "P1: " + player1Score + 
                         "  |  P2: " + player2Score;
    }
}
