using UnityEngine;
public class Rotate : MonoBehaviour
{ [SerializeField] private float rotationSpeed = 20f; // degrees per second

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
}
