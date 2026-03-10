using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SpeedBoast : MonoBehaviour
{
    [Header("Boost Settings")]
    [SerializeField] private float boostMultiplier = 2f;
    [SerializeField] private float boostDuration = 3f;
    [SerializeField] private AudioClip pickupSound;
    
    [Header("Effects")]
    [SerializeField] private ParticleSystem pickupEffect;
    
    private void OnTriggerEnter(Collider other)
    {
        NewMovement movement = other.GetComponent<NewMovement>();
        
        if (movement != null)
        {
            StartCoroutine(ApplyBoost(movement));
            NotifySpawner();
            gameObject.SetActive(false);
        }
    }
    
    private IEnumerator ApplyBoost(NewMovement movement)
    {
        // Play effects
        if (pickupEffect != null)
        {
            ParticleSystem effect = Instantiate(pickupEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, 2f);
        }
        
        // Play sound
        if (pickupSound != null && SoundManager.Instance != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
        
        // Store original values
        float originalMaxSpeed = movement.maxForwardSpeed;
        float originalAcceleration = movement.acceleration;
        
        // Apply boost
        movement.maxForwardSpeed = originalMaxSpeed * boostMultiplier;
        movement.acceleration = originalAcceleration * boostMultiplier;
        
        yield return new WaitForSeconds(boostDuration);
        
        // Revert
        movement.maxForwardSpeed = originalMaxSpeed;
        movement.acceleration = originalAcceleration;
        
        Destroy(gameObject);
    }
    
    void NotifySpawner()
    {
        Randomizer spawner = GetComponentInParent<Randomizer>();

        if (spawner != null)
            spawner.PowerUpCollected();
    }
}