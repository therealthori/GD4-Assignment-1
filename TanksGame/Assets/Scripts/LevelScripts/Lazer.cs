using UnityEngine;

public class Lazer : MonoBehaviour
{
   public Transform[] shootPoints;
    public float maxDistance = 50f;

    public LayerMask stopLayers;
    public LayerMask damageLayers;

    public LineRenderer linePrefab;

    public float damagePerSecond = 25f;

    private LineRenderer[] lines;

    void Start()
    {
        lines = new LineRenderer[shootPoints.Length];

        for (int i = 0; i < shootPoints.Length; i++)
        {
            LineRenderer line = Instantiate(linePrefab, shootPoints[i]);
            line.positionCount = 2;
            lines[i] = line;
        }
    }

    void Update()
    {
        for (int i = 0; i < shootPoints.Length; i++)
        {
            ShootLaser(i);
        }
    }

    void ShootLaser(int index)
    {
        Transform shootPoint = shootPoints[index];
        LineRenderer line = lines[index];

        Vector3 startPos = shootPoint.position;
        Vector3 direction = shootPoint.forward;

        RaycastHit hit;

        Vector3 endPos = startPos + direction * maxDistance;

        if (Physics.Raycast(startPos, direction, out hit, maxDistance))
        {
            endPos = hit.point;

            // Damage player
            if (((1 << hit.collider.gameObject.layer) & damageLayers) != 0)
            {
                Health health = hit.collider.GetComponent<Health>();

                if (health != null)
                {
                    health.TakeDamage(damagePerSecond * Time.deltaTime);
                }
            }

            // Stop at wall layers
            if (((1 << hit.collider.gameObject.layer) & stopLayers) != 0)
            {
                endPos = hit.point;
            }
        }

        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
    }
}
