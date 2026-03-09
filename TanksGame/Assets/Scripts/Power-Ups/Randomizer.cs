using UnityEngine;

public class Randomizer : MonoBehaviour
{
    [Header("Power Up Prefabs")]
        public GameObject[] powerUps;
    
        [Header("Spawn Settings")]
        public float respawnTime = 8f;
    
        private GameObject currentPowerUp;
    
        void Start()
        {
            SpawnPowerUp();
        }
    
        void SpawnPowerUp()
        {
            if (powerUps.Length == 0) return;
    
            int randomIndex = Random.Range(0, powerUps.Length);
    
            currentPowerUp = Instantiate(
                powerUps[randomIndex],
                transform.position,
                Quaternion.identity
            );
    
            currentPowerUp.transform.parent = transform;
        }
    
        public void PowerUpCollected()
        {
            if (currentPowerUp != null)
                Destroy(currentPowerUp);
    
            Invoke(nameof(SpawnPowerUp), respawnTime);
        }
}
