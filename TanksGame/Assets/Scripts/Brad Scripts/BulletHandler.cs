using UnityEngine;
using UnityEngine.InputSystem;

public class BulletHandler : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float fireRate = 0.5f;

    [Header("Fire Points")]
    [SerializeField] private Transform p1FirePoint;
    [SerializeField] private Transform p2FirePoint;

    [Header("Actions")]
    [SerializeField] private InputActionReference p1Shoot;
    [SerializeField] private InputActionReference p2Shoot;


    //private float p1NextFireTime = 0f;
    //private float p2NextFireTime = 0f;

    private float p1Cooldown = 0f;
    private float p2Cooldown = 0f;

    [Header("Recoil Settings")]
    [SerializeField] private float recoilForce = 3f;

    [SerializeField] private Rigidbody p1Rb;
    [SerializeField] private Rigidbody p2Rb;

    [SerializeField] private TankMovement tankMovement;

    private void OnEnable()
    {
        p1Shoot.action.Enable();
        p2Shoot.action.Enable();
    }

    private void OnDisable()
    {
        p1Shoot.action.Disable();
        p2Shoot.action.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var gamepad1 = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
        var gamepad2 = Gamepad.all.Count > 1 ? Gamepad.all[1] : null;

        // Fall back to action references if no gamepad found
        float m1 = p1Shoot.action.ReadValue<float>();
        float m2 = p2Shoot.action.ReadValue<float>();

        // countdown timers
        if (p1Cooldown > 0f) p1Cooldown -= Time.deltaTime;
        if (p2Cooldown > 0f) p2Cooldown -= Time.deltaTime;

        HandleShooting(p1FirePoint, m1, ref p1Cooldown);
        HandleShooting(p2FirePoint, m2, ref p2Cooldown);


        //if (Input.GetKey("space") && Time.time > nextFireTime)
        //{
        //    nextFireTime = Time.time + fireRate;
        //    Shoot();
        //}
    }

    private void HandleShooting(Transform firePoint, float input, ref float cooldown)
    {
        if (firePoint == null) return;

        if (input > 0.1f && cooldown <= 0f)
        {
            cooldown = fireRate;

            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody bulletRb = bulletInstance.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.AddForce(firePoint.forward * bulletSpeed, ForceMode.VelocityChange);
            }

            Destroy(bulletInstance, 3f);
        }
    }
}
