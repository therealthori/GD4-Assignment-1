using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    private LineRenderer lineRenderer;

    // Define the start and end points for the line in the Inspector
    public Vector3 startPoint = new Vector3(0, 0, 0);
    public Vector3 endPoint = new Vector3(0, 0, 10);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Line Renderer component attached to this GameObject
        lineRenderer = GetComponent<LineRenderer>();

        // Set the number of points to 2 (start and end)
        lineRenderer.positionCount = 2;

        // Set the positions of the start and end points
        DrawStraightLine();
    }

    // Update is called once per frame
    void Update()
    {
        DrawStraightLine();
    }

    void DrawStraightLine()
    {
        // Set the first point to the start position
        lineRenderer.SetPosition(0, startPoint);

        // Set the second point to the end position
        lineRenderer.SetPosition(1, endPoint);
    }
}
