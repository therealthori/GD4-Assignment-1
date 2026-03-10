using UnityEngine;

public class BulletDamage : MonoBehaviour
{
     [Header("Damage")]
    public float damage = 100f;
    public string damageTag = "Player";

    [Header("Tags")]
    public string destructibleTag = "destructible";

    [Header("Bounce")]
    public int maxBounces = 2;
    private int bounceCount = 0;

    [Header("Effects")]
    public GameObject destroyEffect;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet hit: " + collision.collider.name);

        // DAMAGE PLAYER
        if (collision.collider.CompareTag(damageTag))
        {
            Health health = collision.collider.GetComponentInParent<Health>();

            if (health != null)
            {
                health.TakeDamage(damage);

                // player hit sound
                SoundManager.Instance.PlaySound(SoundManager.Instance.playerDeath);

                Debug.Log("Bullet damaged player");
            }
        }

        // DESTROY DESTRUCTIBLE WALL
        if (collision.collider.CompareTag(destructibleTag))
        {
            // play wall break sound
            SoundManager.Instance.PlaySoundWithVolume(SoundManager.Instance.wallBreak, 0.3f);

            Destroy(collision.collider.gameObject);

            if (destroyEffect != null)
                Instantiate(destroyEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
            return;
        }

        bounceCount++;

        if (bounceCount <= maxBounces)
        {
            // bounce sound
            SoundManager.Instance.PlaySoundWithVolume(SoundManager.Instance.bulletbounce, 0.3f);
        }

        if (bounceCount > maxBounces)
        {
            // bullet break sound
            SoundManager.Instance.PlaySound(SoundManager.Instance.bulletbreak);

            if (destroyEffect != null)
                Instantiate(destroyEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
