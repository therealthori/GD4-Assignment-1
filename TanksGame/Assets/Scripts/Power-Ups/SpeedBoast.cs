using UnityEngine;

public class SpeedBoast : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
        {
            NewMovement movement = other.GetComponent<NewMovement>();
    
            if (movement != null)
            {
                movement.ActivateBoost();
                Destroy(gameObject);
            }
        }
        
        void NotifySpawner()
            {
                Randomizer spawner = GetComponentInParent<Randomizer>();
        
                if (spawner != null)
                {
                    spawner.PowerUpCollected();
                }
            }
}
