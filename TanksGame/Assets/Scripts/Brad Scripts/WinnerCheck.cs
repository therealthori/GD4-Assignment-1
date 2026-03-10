using UnityEngine;

public class WinnerCheck : MonoBehaviour
{
    public GameObject player1WinImage;
    public GameObject player2WinImage;

    public ScoreManager scoreMan;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int winner = ScoreManager.Instance.GameWinner;

        if (winner == 1)
        {
            player1WinImage.SetActive(true);
            player2WinImage.SetActive(false);
            scoreMan.ResetScores();

        }
        else if (winner == 2)
        {
            player1WinImage.SetActive(false);
            player2WinImage.SetActive(true);
            scoreMan.ResetScores();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
