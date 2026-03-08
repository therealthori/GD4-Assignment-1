using UnityEngine;
public class Rotate : MonoBehaviour
{ [SerializeField] private float rotationSpeed = 20f; // degrees per second

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

     void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Keep the tank attached while it remains on the platform
            if (collision.transform.parent != transform)
                collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
