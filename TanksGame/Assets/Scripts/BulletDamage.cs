using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [Header("Damage")]
    public float damage = 100f;
    public string damageTag = "Player";

    [Header("Tags")]
    public string destructibleTag = "destructible";

    [Header("Bounce")]
    public int maxBounces = 1;
    private int bounceCount = 0;

    [Header("Effects")]
    public GameObject destroyEffect;

    [Header("Sound")]
    public AudioClip bounceSound;
    public AudioClip explodeSound;
    private AudioSource audioSource;

  void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet hit: " + collision.collider.name);

        // DAMAGE PLAYER (same idea as laser)
        if (collision.collider.CompareTag(damageTag))
        {
            Health health = collision.collider.GetComponentInParent<Health>();

            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log("Bullet damaged player");
            }
        }

       if (collision.collider.CompareTag(destructibleTag))
{
    Destroy(collision.collider.gameObject);
    Debug.Log("Destroyed destructible object");

    if (destroyEffect != null)
        Instantiate(destroyEffect, transform.position, Quaternion.identity);

    if (explodeSound != null)
        AudioSource.PlayClipAtPoint(explodeSound, transform.position);

    Destroy(gameObject); // despawn bullet
    return; 
}

        bounceCount++;

         if (bounceCount <= maxBounces)
        {
            if (audioSource != null && bounceSound != null)
                audioSource.PlayOneShot(bounceSound);
        }

        if (bounceCount > maxBounces)
        {
            if (audioSource != null && explodeSound != null)
                audioSource.PlayOneShot(explodeSound);

            if (destroyEffect != null)
            {
                Instantiate(destroyEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
