using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    [System.Serializable]
    public class PlayerScore
    {
        public int playerNumber;
        public int score;
        public string playerName;
    }
    
    public PlayerScore[] playerScores = new PlayerScore[2];
    public int roundsToWin = 3; // Best of 5 rounds
    public float roundEndDelay = 3f;
    
    // Events for UI updates
    public System.Action OnScoresUpdated;
    public System.Action<int> OnRoundWinner;
    public System.Action<int> OnGameWinner;
    
    void Awake()
    {
        // Singleton pattern - keep this object between scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void InitializeScores()
    {
        playerScores[0] = new PlayerScore { playerNumber = 1, score = 0, playerName = "Player 1" };
        playerScores[1] = new PlayerScore { playerNumber = 2, score = 0, playerName = "Player 2" };
    }
    
    public void PlayerDied(int deadPlayerNumber)
    {
        // The surviving player wins the round
        int winningPlayer = (deadPlayerNumber == 1) ? 2 : 1;
        
        // Add score to winner
        AddScore(winningPlayer);
        
        // Trigger events
        OnRoundWinner?.Invoke(winningPlayer);
        
        // Check if someone won the game
        int winner = CheckGameWinner();
        if (winner > 0)
        {
            OnGameWinner?.Invoke(winner);
            StartCoroutine(LoadGameWinnerScene(winner));
        }
        else
        {
            // Load next round
            StartCoroutine(LoadNextRound());
        }
    }
    
    void AddScore(int playerNumber)
    {
        foreach (var playerScore in playerScores)
        {
            if (playerScore.playerNumber == playerNumber)
            {
                playerScore.score++;
                OnScoresUpdated?.Invoke();
                break;
            }
        }
    }
    
    int CheckGameWinner()
    {
        foreach (var playerScore in playerScores)
        {
            if (playerScore.score >= roundsToWin)
            {
                return playerScore.playerNumber;
            }
        }
        return -1; // No winner yet
    }
    
    IEnumerator LoadNextRound()
    {
        yield return new WaitForSeconds(roundEndDelay);
        
        // Load the next round scene (you can cycle through different scenes)
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        // If we're at the last round scene, loop back to first
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 1; // Assuming scene 0 is main menu
        }
        
        SceneManager.LoadScene(nextSceneIndex);
    }
    
    IEnumerator LoadGameWinnerScene(int winner)
    {
        yield return new WaitForSeconds(roundEndDelay);
        
        // Load victory scene (you can create a separate victory scene)
        // For now, just reset scores and go back to first level
        ResetScores();
        SceneManager.LoadScene(1); // Load first round
    }
    
    public void ResetScores()
    {
        foreach (var playerScore in playerScores)
        {
            playerScore.score = 0;
        }
        OnScoresUpdated?.Invoke();
    }
    
    public int GetPlayerScore(int playerNumber)
    {
        foreach (var playerScore in playerScores)
        {
            if (playerScore.playerNumber == playerNumber)
                return playerScore.score;
        }
        return 0;
    }
}