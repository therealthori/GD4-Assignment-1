using UnityEngine;

public class Shield : MonoBehaviour
{
    public float shieldDuration = 4f;
    
        private void OnTriggerEnter(Collider other)
        {
            Health health = other.GetComponent<Health>();
    
            if (health != null)
            {
                health.ActivateShield(shieldDuration);
                Destroy(gameObject);
            }
        }
}
