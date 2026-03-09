using UnityEngine;

public class NewTrajectoryLine : MonoBehaviour
{
    [SerializeField] private int maxBounces = 1;
    [SerializeField] private int linePoints = 25;
    [SerializeField] private LineRenderer aimLineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(aimLineRenderer);
        Vector3 rayPos = transform.position;
        Vector3 rayDir = transform.forward;
        aimLineRenderer.positionCount = 1;
        aimLineRenderer.SetPosition(0, rayPos);

        for(int i=0; i < maxBounces; i++)
        {
            if(Physics.Raycast(rayPos,rayDir, out RaycastHit hit))
            {
                aimLineRenderer.positionCount++;
                aimLineRenderer.SetPosition(aimLineRenderer.positionCount - 1, hit.point);
                rayDir = Vector3.Reflect(rayDir, hit.normal);
                rayPos = hit.point;
            }
            else
            {
                aimLineRenderer.positionCount++;
                aimLineRenderer.SetPosition(aimLineRenderer.positionCount - 1, rayPos + rayDir * 100);
                break;
            }
        }
    }
}
