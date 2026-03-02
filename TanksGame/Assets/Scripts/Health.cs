using UnityEngine;

public class Health : MonoBehaviour
{
    public int playerNumber; // 1 or 2
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        ScoreManager.Instance.PlayerDied(playerNumber);
        gameObject.SetActive(false);
    }
    
}
