using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [System.Serializable]
    public class PlayerScore
    {
        public int    playerNumber;
        public int    score;
        public string playerName;
    }

    public PlayerScore[] playerScores   = new PlayerScore[2];
    public int roundsToWin   = 3;
    public float roundEndDelay = 3f;

    //public NewAim aim
    //public newBulletHandel
    //public NewMovement

    public GameObject player1;
    public GameObject player2;

    // Stores the winner so WinScene can read it
    public int GameWinner { get; private set; } = 0;

    public System.Action       OnScoresUpdated;
    public System.Action<int>  OnRoundWinner;
    public System.Action<int>  OnGameWinner;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeScores();
        }
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    void InitializeScores()
    {
        playerScores[0] = new PlayerScore { playerNumber = 1, score = 0, playerName = "Player 1" };
        playerScores[1] = new PlayerScore { playerNumber = 2, score = 0, playerName = "Player 2" };
    }

    public void PlayerDied(int deadPlayerNumber)
    {
        int winningPlayer = (deadPlayerNumber == 1) ? 2 : 1;

        AddScore(winningPlayer);
        OnRoundWinner?.Invoke(winningPlayer);

        int winner = CheckGameWinner();
        if (winner > 0)
        {
            GameWinner = winner; // store winner before scene change
            OnGameWinner?.Invoke(winner);
            StartCoroutine(LoadWinScene());
            player1.SetActive(false);
            player2.SetActive(false);
        }
        else
        {
            StartCoroutine(LoadNextRound());
        }
    }

    void AddScore(int playerNumber)
    {
        foreach (var ps in playerScores)
        {
            if (ps.playerNumber == playerNumber)
            {
                ps.score++;
                OnScoresUpdated?.Invoke();
                break;
            }
        }
    }

    int CheckGameWinner()
    {
        foreach (var ps in playerScores)
            if (ps.score >= roundsToWin) return ps.playerNumber;

        return -1;
    }

    IEnumerator LoadNextRound()
    {
        yield return new WaitForSeconds(roundEndDelay);

        int next = SceneManager.GetActiveScene().buildIndex + 1;

        // Skip scene 0 (MainMenu) and WinScene (last scene)
        int lastPlayableScene = SceneManager.sceneCountInBuildSettings - 2;
        if (next > lastPlayableScene) next = 1;

        SceneManager.LoadScene(next);
    }

    // NEW — loads the dedicated win screen scene
    IEnumerator LoadWinScene()
    {
        yield return new WaitForSeconds(roundEndDelay);
        SceneManager.LoadScene("Level6");
    }

    public void ResetScores()
    {
        GameWinner = 0;
        foreach (var ps in playerScores) ps.score = 0;
        OnScoresUpdated?.Invoke();
    }

    public int GetPlayerScore(int playerNumber)
    {
        foreach (var ps in playerScores)
            if (ps.playerNumber == playerNumber) return ps.score;
        return 0;
    }
}