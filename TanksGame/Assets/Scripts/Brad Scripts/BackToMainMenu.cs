using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    public float delay = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(Return());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //IEnumerator Return()
    //{
    //    yield return new WaitForSeconds(delay);

    //    ScoreManager.Instance.ResetScores();
    //    SceneManager.LoadScene("MainMenu");
    //}
}
