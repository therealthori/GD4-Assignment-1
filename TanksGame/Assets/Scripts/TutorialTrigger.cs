using UnityEngine;
using TMPro;

public class TutorialTrigger : MonoBehaviour
{
   public TMP_Text tutorialText;

    private void Start()
    {
        
        if (tutorialText != null)
        {
            tutorialText.text = "";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialText.text = "Use the right joystick to aim and R2 to shoot";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialText.text = "";
        }
    }
}
