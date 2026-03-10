using UnityEngine;

public class Randomizer : MonoBehaviour
{
    [Header("PowerUp Prefabs")]
    [SerializeField] private GameObject speedBoostPrefab;
    [SerializeField] private GameObject shieldPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float respawnDelay = 8f;

    private GameObject currentPowerUp;

    void Start()
    {
        SpawnRandomPowerUp();
    }

    void SpawnRandomPowerUp()
    {
        int random = Random.Range(0, 2);

        GameObject prefabToSpawn = null;

        if (random == 0)
            prefabToSpawn = speedBoostPrefab;
        else
            prefabToSpawn = shieldPrefab;

        currentPowerUp = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

        // Make spawned pickup a child of the spawner
        currentPowerUp.transform.parent = transform;
    }

    public void PowerUpCollected()
    {
        if (currentPowerUp != null)
            Destroy(currentPowerUp);

        Invoke(nameof(SpawnRandomPowerUp), respawnDelay);
    }
}