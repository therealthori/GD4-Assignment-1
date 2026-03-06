using UnityEngine;

public class MissileFall : MonoBehaviour
{
     public float fallSpeed = 20f;
    public float groundY = 0f;

    [Header("Explosion")]
    public ParticleSystem explosionParticles;

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y <= groundY)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (explosionParticles != null)
        {
            ParticleSystem explosion = Instantiate(
                explosionParticles,
                transform.position,
                Quaternion.identity
            );

            explosion.Play();
            Destroy(explosion.gameObject, explosion.main.duration);
        }

        Destroy(gameObject);
    }
}
