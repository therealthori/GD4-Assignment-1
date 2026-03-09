using UnityEngine;
using UnityEngine.InputSystem;

public class newBulletHandel : MonoBehaviour
{
     [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float fireRate = 0.5f;

    [Header("Fire Point")]
    [SerializeField] private Transform firePoint;

    [Header("Input")]
    [SerializeField] private InputActionReference shootAction;

    [Header("Magazine Settings")]
    [SerializeField] private int magazineSize = 5;
    [SerializeField] private float reloadTime = 2f;

    private int ammo;
    private bool reloading = false;
    private float reloadTimer = 0f;
    private float cooldown = 0f;

    [Header("Recoil")]
    [SerializeField] private float recoilForce = 3f;
    [SerializeField] private Rigidbody tankRb;

    [Header("Effects")]
    [SerializeField] private ParticleSystem bulletShotEffect;

    private void OnEnable()
    {
        shootAction.action.Enable();
    }

    private void OnDisable()
    {
        shootAction.action.Disable();
    }

    private void Start()
    {
        ammo = magazineSize;
    }

    void Update()
    {
        float input = shootAction.action.ReadValue<float>();

        UpdateReload();

        if (cooldown > 0f)
            cooldown -= Time.deltaTime;

        HandleShooting(input);
    }

    private void HandleShooting(float input)
    {
        if (firePoint == null || reloading) return;

        if (input > 0.1f && cooldown <= 0f && ammo > 0)
        {
            cooldown = fireRate;
            ammo--;

            if (bulletShotEffect != null)
                bulletShotEffect.Play();

            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody bulletRb = bulletInstance.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.AddForce(firePoint.forward * bulletSpeed, ForceMode.VelocityChange);
            }

            // recoil
            if (tankRb != null)
            {
                tankRb.AddForce(-firePoint.forward * recoilForce, ForceMode.Impulse);
            }

            Destroy(bulletInstance, 3f);

            if (ammo <= 0)
            {
                reloading = true;
                reloadTimer = reloadTime;
            }
        }
    }

    private void UpdateReload()
    {
        if (!reloading) return;

        reloadTimer -= Time.deltaTime;

        if (reloadTimer <= 0f)
        {
            reloading = false;
            ammo = magazineSize;
        }
    }

    public float GetReloadPercent()
    {
        if (!reloading) return 1f;
        return 1f - (reloadTimer / reloadTime);
    }

    public int GetAmmo()
    {
        return ammo;
    }
}
