using UnityEngine;
using System.Collections;

public class Randomizer : MonoBehaviour
{
    [Header("PowerUps")]
    public GameObject[] powerUps;
    
    [Header("Spawn Settings")]
    public float spawnDelay = 5f;
    
    private GameObject currentPowerUp;
    
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
    
    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (currentPowerUp == null)
           {
                 SpawnPowerUp();
           }
    
               yield return new WaitForSeconds(spawnDelay);
            }
        }
    
    void SpawnPowerUp()
        {
            int randomIndex = Random.Range(0, powerUps.Length);
    
            currentPowerUp = Instantiate( powerUps[randomIndex], transform.position, Quaternion.identity);
        }
}
