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
    [SerializeField] private int playerIndex = 0; // 0 for Player 1, 1 for Player 2
                                                  // REMOVED: [SerializeField] private InputActionReference shootAction; - Don't use shared action references!

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

    private void Start()
    {
        ammo = magazineSize;
        AssignGamepad();
    }

    private void AssignGamepad()
    {
        // Clear previous assignment
        assignedGamepad = null;

        // Assign the appropriate gamepad based on player index
        if (Gamepad.all.Count > playerIndex)
        {
            assignedGamepad = Gamepad.all[playerIndex];
            Debug.Log($"Player {playerIndex + 1} shooter assigned to {assignedGamepad.name}");
        }
        else
        {
            Debug.Log($"Player {playerIndex + 1} shooter: No gamepad assigned, using keyboard");
        }
    }

    void Update()
    {
        // Reassign gamepad if device count changes
        if (assignedGamepad == null && Gamepad.all.Count > playerIndex)
        {
            AssignGamepad();
        }
        // Clear gamepad if it was unplugged
        else if (assignedGamepad != null && Gamepad.all.Count <= playerIndex)
        {
            assignedGamepad = null;
        }

        // Get input DIRECTLY from devices, NOT through shared action references
        bool shouldShoot = GetShootInput();

        UpdateReload();

        if (cooldown > 0f)
            cooldown -= Time.deltaTime;

        if (shouldShoot)
        {
            HandleShooting();
        }
    }

    private bool GetShootInput()
    {
        // Player 1: Use keyboard (WASD + Space/Left Control)
        if (playerIndex == 0)
        {
            // Check gamepad first if available
            if (assignedGamepad != null)
            {
                if (assignedGamepad.rightTrigger.ReadValue() > 0.1f ||
                    assignedGamepad.aButton.isPressed ||
                    assignedGamepad.xButton.isPressed)
                {
                    return true;
                }
            }

            // Keyboard fallback for Player 1
            if (Keyboard.current != null)
            {
                // Space or Left Control for Player 1
                if (Keyboard.current.spaceKey.isPressed ||
                    Keyboard.current.leftCtrlKey.isPressed)
                {
                    return true;
                }
            }
        }
        // Player 2: Use keyboard (Arrow Keys + Enter/Right Control)
        else if (playerIndex == 1)
        {
            // Check gamepad first if available
            if (assignedGamepad != null)
            {
                if (assignedGamepad.rightTrigger.ReadValue() > 0.1f ||
                    assignedGamepad.aButton.isPressed ||
                    assignedGamepad.xButton.isPressed)
                {
                    return true;
                }
            }

            // Keyboard fallback for Player 2
            if (Keyboard.current != null)
            {
                // Enter or Right Control for Player 2
                if (Keyboard.current.enterKey.isPressed ||
                    Keyboard.current.rightCtrlKey.isPressed)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void HandleShooting()
    {
        if (firePoint == null || reloading) return;

        if (cooldown <= 0f && ammo > 0)
        {
            cooldown = fireRate;
            ammo--;
            float pitch = Mathf.Lerp(1f, 1.8f, 1f - ((float)ammo / magazineSize));
            SoundManager.Instance.PlaySoundWithPitch(SoundManager.Instance.shootSound, pitch);

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
            SoundManager.Instance.PlaySound(SoundManager.Instance.reloadSound);

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
