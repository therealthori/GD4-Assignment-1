using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Shield Settings")]
        public float shieldDuration = 5f;
    
        private void OnTriggerEnter(Collider other)
        {
            Health playerHealth = other.GetComponent<Health>();
    
            if (playerHealth != null)
            {
                playerHealth.ActivateShield(shieldDuration);
    
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
