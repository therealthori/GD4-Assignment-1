using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [SerializeField] private float p1Cooldown = 0f;
    [SerializeField] private float p2Cooldown = 0f;

    //[SerializeField] private (note, this is for the reload UI)

    [Header("Magazine Settings")]
    [SerializeField] private int magazineSize = 5;
    [SerializeField] private float reloadTime = 2f;

    [SerializeField] private int p1Ammo;
    [SerializeField] private int p2Ammo;

    [SerializeField] private float p1ReloadTimer = 0f;
    [SerializeField] private float p2ReloadTimer = 0f;

    [SerializeField] private bool p1Reloading = false;
    [SerializeField] private bool p2Reloading = false;


    [Header("Recoil Settings")]
    [SerializeField] private float recoilForce = 3f;

    [SerializeField] private Rigidbody p1Rb;
    [SerializeField] private Rigidbody p2Rb;

    [SerializeField] private TankMovement tankMovement;

    [SerializeField] private ParticleSystem bulletShotEffect;

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
        p1Ammo = magazineSize;
        p2Ammo = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        var gamepad1 = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
        var gamepad2 = Gamepad.all.Count > 1 ? Gamepad.all[1] : null;

        // Fall back to action references if no gamepad found
        float m1 = p1Shoot.action.ReadValue<float>();
        float m2 = p2Shoot.action.ReadValue<float>();

        UpdateReload(ref p1ReloadTimer, ref p1Reloading, ref p1Ammo);
        UpdateReload(ref p2ReloadTimer, ref p2Reloading, ref p2Ammo);

        if (p1Cooldown > 0f) p1Cooldown -= Time.deltaTime;
        if (p2Cooldown > 0f) p2Cooldown -= Time.deltaTime;

        // countdown timers
        if (p1Cooldown > 0f) p1Cooldown -= Time.deltaTime;
        if (p2Cooldown > 0f) p2Cooldown -= Time.deltaTime;

        HandleShooting(p1FirePoint, m1, ref p1Cooldown, ref p1Ammo, ref p1Reloading, ref p1ReloadTimer);
        HandleShooting(p2FirePoint, m2, ref p2Cooldown, ref p2Ammo, ref p2Reloading, ref p2ReloadTimer);


        //if (Input.GetKey("space") && Time.time > nextFireTime)
        //{
        //    nextFireTime = Time.time + fireRate;
        //    Shoot();
        //}
    }

    private void HandleShooting(Transform firePoint, float input, ref float cooldown, ref int ammo, ref bool reloading, ref float reloadTimer)
{
        if (firePoint == null || reloading) return;

        if (input > 0.1f && cooldown <= 0f && ammo > 0)
        {
            cooldown = fireRate;
            ammo--;
            //bulletShotEffect.Play();

            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody bulletRb = bulletInstance.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.AddForce(firePoint.forward * bulletSpeed, ForceMode.VelocityChange);
            }

            Destroy(bulletInstance, 3f);
            //bulletShotEffect.Stop();

            if (ammo <= 0)
            {
                reloading = true;
                reloadTimer = reloadTime;
            }
        }
    }

    public float GetP1ReloadPercent()
    {
        if (!p1Reloading) return 1f;
        return 1f - (p1ReloadTimer / reloadTime);
    }
    public int GetP1Ammo()
    {
        return p1Ammo;
    }

    private void UpdateReload(ref float reloadTimer, ref bool reloading, ref int ammo)
    {
        if (!reloading) return;

        reloadTimer -= Time.deltaTime;

        if (reloadTimer <= 0f)
        {
            reloading = false;
            ammo = magazineSize;
        }
    }
}
