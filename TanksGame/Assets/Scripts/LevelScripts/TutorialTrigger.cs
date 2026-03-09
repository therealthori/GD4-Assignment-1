using UnityEngine;
using TMPro;

public class TutorialTrigger : MonoBehaviour
{
   public GameObject p1MovementTutorialText;
   //public TMP_Text p2MovementTutorialText;
   public GameObject p1ShootTutorialText;
   //public TMP_Text p2ShootTutorialText;

    private void Start()
    {
        
        //if (tutorialText != null)
        //{
        //    tutorialText.text = "";
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            p1MovementTutorialText.SetActive(false); //= "Use the right joystick to aim and R2 to shoot";
            p1ShootTutorialText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    tutorialText.text = "";
        //}
    }
}
