using UnityEngine;

public class SpeedBoast : MonoBehaviour
{
    public float boostMultiplier = 2f;
    public float boostDuration = 3f;
    
    private void OnTriggerEnter(Collider other)
    {
       Health health = other.GetComponent<Health>();
        
        if (health != null)
        {
             int playerNumber = health.playerNumber;
        
             TankMovement movement = FindObjectOfType<TankMovement>();
             movement.ActivateSpeedBoost(playerNumber, boostMultiplier, boostDuration);
        
             Destroy(gameObject);
        }
    }
}
