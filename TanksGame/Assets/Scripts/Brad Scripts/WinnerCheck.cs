using UnityEngine;

public class WinnerCheck : MonoBehaviour
{
    public GameObject player1WinImage;
    public GameObject player2WinImage;

    void Start()
    {
        // Determine winner first
        ScoreManager.Instance.DetermineGameWinner();
        int winner = ScoreManager.Instance.GameWinner;

        // Show correct image
        player1WinImage.SetActive(winner == 1);
        player2WinImage.SetActive(winner == 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
