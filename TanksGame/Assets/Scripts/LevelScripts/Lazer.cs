using UnityEngine;
using System.Collections;

public class Lazer : MonoBehaviour
{
     [Header("Laser Setup")]
    public Transform[] shootPoints;
    public float maxDistance = 50f;
    public LayerMask stopLayers;
    public LineRenderer linePrefab;

    [Header("Damage")]
    public float damage = 100f;             
    public string damageTag = "Player";

    [Header("Particles")]
    public GameObject hitParticles;

    [Header("Sound")]
    public AudioSource laserAudioSource;
    public AudioClip laserSound;

    private LineRenderer[] lines;
    private bool hasDamagedPlayer = false; // only damage once

    void Start()
    {
        lines = new LineRenderer[shootPoints.Length];

        for (int i = 0; i < shootPoints.Length; i++)
        {
            LineRenderer line = Instantiate(linePrefab, shootPoints[i]);
            line.positionCount = 2;
            lines[i] = line;
        }

        // Play laser sound if assigned
        if (laserAudioSource != null && laserSound != null)
        {
            laserAudioSource.clip = laserSound;
            laserAudioSource.loop = true;
            laserAudioSource.Play();
        }
    }

    void Update()
    {
        for (int i = 0; i < shootPoints.Length; i++)
        {
            ShootLaser(i);
        }
    }

    void ShootLaser(int index)
    {
        Transform shootPoint = shootPoints[index];
        LineRenderer line = lines[index];

        Vector3 startPos = shootPoint.position;
        Vector3 direction = shootPoint.forward;
        Vector3 endPos = startPos + direction * maxDistance;

        // Draw debug ray in Scene view
        Debug.DrawRay(startPos, direction * maxDistance, Color.red);

        if (Physics.Raycast(startPos, direction, out RaycastHit hit, maxDistance))
        {
            endPos = hit.point;

            Debug.Log("Laser hit: " + hit.collider.name);

            // Deal 100 damage if it's the player
            if (!hasDamagedPlayer && hit.collider.CompareTag(damageTag))
            {
                Health playerHealth = hit.collider.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage); // This will trigger explosion if health hits 0
                    Debug.Log("Laser damaged player: " + hit.collider.name);
                }

                // Spawn hit particles
                if (hitParticles != null)
                    Instantiate(hitParticles, hit.point, Quaternion.LookRotation(hit.normal));

                hasDamagedPlayer = true; // only damage once
            }

            // Stop laser at walls
            if (((1 << hit.collider.gameObject.layer) & stopLayers) != 0)
            {
                endPos = hit.point;
            }
        }

        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
    }
}
