using UnityEngine;

public class Randomizer : MonoBehaviour
{
    {
        [Header("Shield Settings")]
        public float shieldDuration = 5f;
    
        private void OnTriggerEnter(Collider other)
        {
            // Try to get player components
            NewMovement movement = other.GetComponent<NewMovement>();
            Health health = other.GetComponent<Health>();
    
            if (movement == null && health == null) return;
    
            // Randomly choose power-up
            int randomPower = Random.Range(0, 2); // 0 = Speed, 1 = Shield
    
            if (randomPower == 0 && movement != null)
            {
                movement.ActivateBoost();
                Debug.Log("Speed Boost Activated!");
            }
            else if (randomPower == 1 && health != null)
            {
                health.ActivateShield(shieldDuration);
                Debug.Log("Shield Activated!");
            }
    
            NotifySpawner();
            Destroy(gameObject);
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
}
