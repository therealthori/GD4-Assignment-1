using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int playerNumber; // 1 or 2
    public float maxHealth = 100f;
    private float currentHealth;
    
    public GameObject deathEffect;
    private bool isShielded = false;
    
    [Header("Shield Visual")]
    public GameObject shieldVisual;

    void Start()
    {
        currentHealth = maxHealth;
        
        if (shieldVisual != null)
                    shieldVisual.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        if (isShielded)
                    return;
        
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void ActivateShield(float duration)
        {
            StartCoroutine(ShieldRoutine(duration));
        }
    
        private IEnumerator ShieldRoutine(float duration)
        {
            isShielded = true;
    
            if (shieldVisual != null)
                shieldVisual.SetActive(true);
    
            yield return new WaitForSeconds(duration);
    
            isShielded = false;
    
            if (shieldVisual != null)
                shieldVisual.SetActive(false);
        }

    void Die()
    {
         if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
        }

        ScoreManager.Instance.PlayerDied(playerNumber);
        gameObject.SetActive(false);
    }
    
}
