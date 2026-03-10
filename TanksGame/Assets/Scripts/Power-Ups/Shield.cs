using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{
    [Header("Shield Settings")]
    [SerializeField] private float shieldDuration = 5f;
    [SerializeField] private bool isPermanentPickup = false; 
    [SerializeField] private float respawnTime = 10f; 
    
    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem pickupEffect;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private GameObject shieldVisualPrefab; 
    
    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float bobSpeed = 1f;
    [SerializeField] private float bobHeight = 0.5f;
    
    private Vector3 startPosition;
    private float bobOffset;
    
    private void Start()
    {
        startPosition = transform.position;
        bobOffset = Random.Range(0f, Mathf.PI * 2); 
    }
    
    private void Update()
    {
        // Make the pickup rotate and bob
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        // Bobbing motion
        Vector3 newPosition = startPosition;
        newPosition.y += Mathf.Sin((Time.time + bobOffset) * bobSpeed) * bobHeight;
        transform.position = newPosition;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has a Health component
        Health playerHealth = other.GetComponent<Health>();

        if (playerHealth != null)
        {
            // Activate shield on the player
            playerHealth.ActivateShield(shieldDuration);
            NotifySpawner();
            
            // Play pickup effects
            PlayPickupEffects();
            
            // Handle pickup disappearance
            if (isPermanentPickup)
            {
                StartCoroutine(RespawnPickup());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    
    private void PlayPickupEffects()
    {
        // Play particle effect
        if (pickupEffect != null)
        {
            ParticleSystem effect = Instantiate(pickupEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, 2f);
        }
        
        // Play sound
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
    }
    
    private IEnumerator RespawnPickup()
    {
        // Disable visuals and collider
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        
        // Wait for respawn time
        yield return new WaitForSeconds(respawnTime);
        
        // Re-enable visuals and collider
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
    
    void NotifySpawner()
    {
        Randomizer spawner = GetComponentInParent<Randomizer>();

        if (spawner != null)
            spawner.PowerUpCollected();
    }
}