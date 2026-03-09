using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private Transform gun;
    [SerializeField] private float aimDistance = 25f;

    [SerializeField] private float startOffset = 0.5f;

    [SerializeField] private Color nonTargetColor = Color.white;
    [SerializeField] private Color enemyColor = Color.red;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAimLine();
    }

    private void UpdateAimLine()
    {
        if (gun == null || aimLine == null) return;

        Vector3 start = gun.position + gun.forward * startOffset;
        Vector3 direction = gun.forward;

        Ray ray = new Ray(start, direction);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, aimDistance))
        {
            aimLine.SetPosition(0, start);
            aimLine.SetPosition(1, hit.point);

            if (hit.collider.CompareTag("Player"))
            {
                aimLine.startColor = enemyColor;
                aimLine.endColor = enemyColor;
            }
            else
            {
                aimLine.startColor = nonTargetColor;
                aimLine.endColor = nonTargetColor;
            }
        }
        else
        {
            Vector3 end = start + direction * aimDistance;

            aimLine.SetPosition(0, start);
            aimLine.SetPosition(1, end);

            aimLine.startColor = nonTargetColor;
            aimLine.endColor = nonTargetColor;
        }
    }
}
