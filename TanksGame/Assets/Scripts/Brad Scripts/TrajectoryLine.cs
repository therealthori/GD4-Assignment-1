using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private Transform gun;
    [SerializeField] private float aimDistance = 25f;

    [SerializeField] private Color nonTargetColor = Color.white;
    [SerializeField] private Color enemyColor = Color.red;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateAimLine()
    {
        if (gun == null || aimLine == null) return;

        Vector3 start = gun.position;
        Vector3 direction = gun.forward;

        Ray ray = new Ray(start, direction);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, aimDistance))
        {
            aimLine.SetPosition(0, start);
            aimLine.SetPosition(1, hit.point);

            if (hit.collider.CompareTag("Tank"))
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

        }
    }
}
