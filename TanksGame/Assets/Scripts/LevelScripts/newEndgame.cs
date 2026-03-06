using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class newEndgame : MonoBehaviour
{
    [Header("Strike Points")]
    public Transform[] targetPoints;           // Your empty GameObjects (landing spots)
    public float spawnHeight = 25f;            // Height above target to spawn missile

    [Header("Prefabs")]
    public ParticleSystem warningParticlePrefab;  // Single particle prefab to instantiate
    public GameObject missilePrefab;
    public ParticleSystem impactParticlePrefab;

    [Header("Timing")]
    public float warningTime = 1.5f;           // How long particles play before missile
    public float startInterval = 5f;           // Initial time between spawns
    public float minimumInterval = 1f;         // Fastest spawn rate
    public float difficultyRamp = 0.15f;       // How much faster each spawn gets

    [Header("Start Control")]
    public bool startSpawning = false;         // Toggle from other scripts
    public float startDelay = 3f;              // Delay before first missile

    private float currentInterval;
    private bool isRunning = false;
    private Coroutine spawnCoroutine;

    void Start()
    {
         currentInterval = startInterval;
    StartCoroutine(StartAfterDelay());
    }

    void Update()
    {
        // Allow runtime toggle
        if (startSpawning && !isRunning)
        {
            BeginMissiles();
        }
    }

    IEnumerator StartAfterDelay()
    {
       yield return new WaitForSeconds(startDelay);

    startSpawning = true;
    }

    public void BeginMissiles()
    {
        if (isRunning) return;
        isRunning = true;
        spawnCoroutine = StartCoroutine(StrikeLoop());
    }

    public void StopMissiles()
    {
        isRunning = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    IEnumerator StrikeLoop()
    {
        while (isRunning)
    {
       
        StartCoroutine(DoStrike());
        
        
        yield return new WaitForSeconds(currentInterval);
        
        // Gradually increase difficulty
        currentInterval = Mathf.Max(minimumInterval, currentInterval - difficultyRamp);
    }
    }

    IEnumerator DoStrike()
    {
        if (targetPoints.Length == 0 || warningParticlePrefab == null || missilePrefab == null)
        {
            Debug.LogWarning("Missing targets, particle prefab, or missile prefab!");
            yield break;
        }

        // Pick random target
        int randomIndex = Random.Range(0, targetPoints.Length);
        Transform target = targetPoints[randomIndex];

        Vector3 groundPosition = target.position;
        Vector3 spawnPosition = groundPosition + Vector3.up * spawnHeight;

        // Instantiate and play warning particle 
        ParticleSystem warning = Instantiate(
            warningParticlePrefab,
            groundPosition,
            Quaternion.identity,
            target  // Parent to target so it stays in place
        );
        warning.Play();

        yield return new WaitForSeconds(warningTime);

        // Spawn missile
        SpawnMissile(spawnPosition, groundPosition);

        // Cleanup particle
        Destroy(warning.gameObject, warning.main.duration);
    }

    void SpawnMissile(Vector3 spawnPos, Vector3 targetPos)
    {
        GameObject missile = Instantiate(missilePrefab, spawnPos, Quaternion.identity);

        // Set the target and particle reference (that's it!)
        MissileImpact impact = missile.GetComponent<MissileImpact>();
        if (impact != null)
        {
            impact.targetPosition = targetPos;
            impact.impactParticlePrefab = impactParticlePrefab;
        }

        Rigidbody rb = missile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (targetPos - spawnPos).normalized;
            rb.linearVelocity = direction * 20f;
        }
    }

}
