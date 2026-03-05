using UnityEngine;

public class rotate : MonoBehaviour
{
    public float rotationSpeed = 20f; // degrees per second

    void Update()
    {
        transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime, Space.World);
    }
}
