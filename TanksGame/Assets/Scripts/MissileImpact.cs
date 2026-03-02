using UnityEngine;

public class MissileImpact : MonoBehaviour
{
     public Vector3 targetPosition;
    public ParticleSystem impactParticlePrefab;
    
    void OnCollisionEnter(Collision collision)
    {
      // Get EXACT target position from collision point
    Vector3 explosionPos = targetPosition;
    
    // Snap Y to ground level (fixes terrain/offset issues)
    RaycastHit hit;
    if (Physics.Raycast(targetPosition + Vector3.up * 10f, Vector3.down, out hit, 20f))
    {
        explosionPos.y = hit.point.y;
    }
    
    // Spawn with PARENT = null to avoid transform inheritance
    ParticleSystem explosion = Instantiate(impactParticlePrefab, explosionPos, Quaternion.identity, null);
    explosion.Play();
    Destroy(explosion.gameObject, explosion.main.duration);
    Destroy(gameObject);
    }
}
