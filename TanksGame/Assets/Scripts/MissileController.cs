using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour
{
   [Header("Strike Points")]
    public Transform[] strikePoints;
    public float spawnHeight = 25f;

    [Header("Prefabs")]
    public ParticleSystem warningParticles;
    public GameObject missilePrefab;

    [Header("Timing")]
    public float warningTime = 1.5f;
    public float startInterval = 5f;
    public float minimumInterval = 1f;
    public float difficultyRamp = 0.15f;

    [Header("Start Control")]
    public bool startAutomatically = true;
    public float startDelay = 3f;   // 🔥 time before missiles begin

    private float currentInterval;
    private bool isRunning = false;

    void Start()
    {
        currentInterval = startInterval;

        if (startAutomatically)
        {
            StartCoroutine(StartAfterDelay());
        }
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        BeginMissiles();
    }

    
    public void BeginMissiles()
    {
        if (isRunning) return;

        isRunning = true;
        StartCoroutine(StrikeLoop());
    }

    IEnumerator StrikeLoop()
    {
        while (true)
        {
            yield return StartCoroutine(DoStrike());

            yield return new WaitForSeconds(currentInterval);

            currentInterval = Mathf.Max(
                minimumInterval,
                currentInterval - difficultyRamp
            );
        }
    }

    IEnumerator DoStrike()
    {
        if (strikePoints.Length == 0)
            yield break;

        // pick random strike point
        Transform chosenPoint = strikePoints[Random.Range(0, strikePoints.Length)];

        Vector3 groundPosition = chosenPoint.position;
        Vector3 spawnPosition = new Vector3(
            groundPosition.x,
            groundPosition.y + spawnHeight,
            groundPosition.z
        );

        //  warning particle
        ParticleSystem warning = Instantiate(
            warningParticles,
            groundPosition,
            Quaternion.identity
        );

        warning.Play();

        yield return new WaitForSeconds(warningTime);

        // missile spawn
        Instantiate(missilePrefab, spawnPosition, Quaternion.identity);

        warning.Stop();
        Destroy(warning.gameObject, warning.main.duration);
    }
}
