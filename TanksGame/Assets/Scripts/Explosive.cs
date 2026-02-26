using UnityEngine;

public class Explosive : MonoBehaviour
{
    [Header("Damage")]
    public float damage = 50f;

    [Header("Knockback")]
    public float explosionForce = 800f;
    public float explosionRadius = 3f;
    public float upwardsModifier = 1f;

    [Header("Other")]
    public float delayBeforeExplosion = 0.2f;
    public GameObject explosionEffect;

    private bool hasExploded = false;
    private Health targetHealth;
    private Rigidbody targetRb;

    private void OnTriggerEnter(Collider other)
    {
        if (hasExploded) return;

        Health health = other.GetComponent<Health>();
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (health != null && rb != null)
        {
            hasExploded = true;
            targetHealth = health;
            targetRb = rb;

            Invoke(nameof(Explode), delayBeforeExplosion);
        }
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Damage ONLY triggering player
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }

        // Knockback ONLY triggering player
        if (targetRb != null)
        {
            targetRb.AddExplosionForce(
                explosionForce,
                transform.position,
                explosionRadius,
                upwardsModifier,
                ForceMode.Impulse
            );
        }

        Destroy(gameObject);
    }
}
