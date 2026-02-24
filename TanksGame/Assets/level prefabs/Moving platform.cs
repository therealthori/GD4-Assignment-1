using UnityEngine;

public class Movingplatform : MonoBehaviour
{
     public GameObject startPoint;
    public GameObject endPoint;
    public float moveSpeed = 2.0f;

    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        startPos = startPoint.transform.position;
        endPos = endPoint.transform.position;
    }

    void FixedUpdate()
    {
        float time = Mathf.PingPong(Time.time * moveSpeed, 1);
        transform.position = Vector3.Lerp(startPos, endPos, time);
    }
}
