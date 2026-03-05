using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    public int firstRoundSceneIndex = 1;
    
    void Start()
    {
        // Make sure ScoreManager exists
        if (ScoreManager.Instance == null)
        {
            GameObject scoreManagerGO = new GameObject("ScoreManager");
            scoreManagerGO.AddComponent<ScoreManager>();
        }
        
        // Make sure UIManager exists
        if (UIManager.Instance == null)
        {
            GameObject uiManagerGO = new GameObject("UIManager");
            uiManagerGO.AddComponent<UIManager>();
            
            // Setup UI references here (you'll need to assign these in inspector)
            // Or create UI elements programmatically
        }
        
        // Load first round
        SceneManager.LoadScene(firstRoundSceneIndex);
    }
}


