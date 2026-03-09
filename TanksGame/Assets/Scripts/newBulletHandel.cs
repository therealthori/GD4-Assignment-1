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
    [SerializeField] private int playerIndex = 0; // 0 for Player 1, 1 for Player 2

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

    // Store assigned gamepad
    private Gamepad assignedGamepad;

    private void OnEnable()
    {
        if (shootAction != null)
            shootAction.action.Enable();
    }

    private void OnDisable()
    {
        if (shootAction != null)
            shootAction.action.Disable();
    }

    private void Start()
    {
        ammo = magazineSize;
        AssignGamepad();
    }

    private void AssignGamepad()
    {
        // Assign the appropriate gamepad based on player index
        if (Gamepad.all.Count > playerIndex)
        {
            assignedGamepad = Gamepad.all[playerIndex];
            Debug.Log($"Player {playerIndex + 1} shooter assigned to {assignedGamepad.name}");
        }
        else
        {
            assignedGamepad = null;
        }
    }

    void Update()
    {
        // Reassign gamepad if device count changes
        if (assignedGamepad == null && Gamepad.all.Count > playerIndex)
        {
            AssignGamepad();
        }

        float input = GetShootInput();

        UpdateReload();

        if (cooldown > 0f)
            cooldown -= Time.deltaTime;

        HandleShooting(input);
    }

    private float GetShootInput()
    {
        // Try gamepad first (right trigger)
        if (assignedGamepad != null)
        {
            float gamepadInput = assignedGamepad.rightTrigger.ReadValue();
            if (gamepadInput > 0.1f)
            {
                return gamepadInput;
            }
        }

        // Fallback to keyboard based on player index
        if (playerIndex == 0 && Keyboard.current != null)
        {
            // Player 1: Space or Left Control
            if (Keyboard.current.spaceKey.isPressed || Keyboard.current.leftCtrlKey.isPressed)
                return 1f;
        }
        else if (playerIndex == 1 && Keyboard.current != null)
        {
            // Player 2: Enter or Right Control
            if (Keyboard.current.enterKey.isPressed || Keyboard.current.rightCtrlKey.isPressed)
                return 1f;
        }

        // Final fallback to input action reference
        if (shootAction != null)
        {
            return shootAction.action.ReadValue<float>();
        }

        return 0f;
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
