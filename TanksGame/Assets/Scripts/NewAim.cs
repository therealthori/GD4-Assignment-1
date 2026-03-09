using UnityEngine;
using UnityEngine.InputSystem;

public class NewAim : MonoBehaviour
{
      [Header("Input")]
    [SerializeField] private InputActionReference aimAction;

    [Header("Gun")]
    [SerializeField] private Transform gun;

    [Header("Movement")]
    [SerializeField] private float rotateSpeed = 120f;

    [Header("Trajectory")]
    [SerializeField] private LineRenderer line;
    [SerializeField] private float trajectoryDistance = 15f;
    [SerializeField] private int maxBounces = 1;

    [SerializeField] private float scrollSpeed = 2f;

    private void OnEnable()
    {
        aimAction.action.Enable();
    }

    private void OnDisable()
    {
        aimAction.action.Disable();
    }

    private void Update()
    {
        Vector2 input = aimAction.action.ReadValue<Vector2>();

        HandleAim(input);

        DrawTrajectory();

        float offset = Time.time * scrollSpeed;
        line.material.mainTextureOffset = new Vector2(offset, 0);
    }

    private void HandleAim(Vector2 input)
    {
        if (gun == null) return;

        // prevent jitter when stick is near center
        if (input.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);

        gun.rotation = Quaternion.Slerp(
            gun.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    void DrawTrajectory()
    {
        if (gun == null || line == null) return;

        Vector3 position = gun.position;
        Vector3 direction = gun.forward;

        line.positionCount = maxBounces + 2;
        line.SetPosition(0, position);

        int index = 1;

        for (int i = 0; i <= maxBounces; i++)
        {
            RaycastHit hit;

            if (Physics.Raycast(position, direction, out hit, trajectoryDistance))
            {
                line.SetPosition(index, hit.point);

                direction = Vector3.Reflect(direction, hit.normal);
                position = hit.point;

                index++;
            }
            else
            {
                line.SetPosition(index, position + direction * trajectoryDistance);
                break;
            }
        }
    }
}
