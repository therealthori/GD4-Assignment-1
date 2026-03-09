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

    [SerializeField] private float p1Cooldown = 0f;
    [SerializeField] private float p2Cooldown = 0f;

    [Header("Magazine Settings")]
    [SerializeField] private int magazineSize = 5;
    [SerializeField] private float reloadTime = 2f;

    [SerializeField] private int p1Ammo;
    [SerializeField] private int p2Ammo;

    [SerializeField] private float p1ReloadTimer = 0f;
    [SerializeField] private float p2ReloadTimer = 0f;

    [SerializeField] private bool p1Reloading = false;
    [SerializeField] private bool p2Reloading = false;

    [SerializeField] private Rigidbody p1Rb;
    [SerializeField] private Rigidbody p2Rb;

    [SerializeField] private TankMovement tankMovement;

    [SerializeField] private ParticleSystem bulletShotEffect;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        FindPlayers();
        p1Ammo = magazineSize;
        p2Ammo = magazineSize;
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        p1Shoot.action.Enable();
        p2Shoot.action.Enable();
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        p1Shoot.action.Disable();
        p2Shoot.action.Disable();
    }
    
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        FindPlayers();
    }
    
    private void FindPlayers()
    {
        // Store previous references to check if we found new ones
        Transform oldP1FirePoint = p1FirePoint;
        Transform oldP2FirePoint = p2FirePoint;
        Rigidbody oldP1Rb = p1Rb;
        Rigidbody oldP2Rb = p2Rb;
        
        // Clear current references
        p1FirePoint = null;
        p2FirePoint = null;
        p1Rb = null;
        p2Rb = null;
        
        // Find all Health components (players)
        Health[] players = FindObjectsOfType<Health>();
        
        foreach (Health player in players)
        {
            if (player.playerNumber == 1)
            {
                // Set the transform/firepoint (you might want a dedicated fire point child object)
                p1FirePoint = player.transform;
                
                // Try to find a dedicated fire point child first (better for bullet spawning)
                Transform firePointChild = player.transform.Find("FirePoint");
                if (firePointChild != null)
                {
                    p1FirePoint = firePointChild;
                    Debug.Log("Found Player 1 dedicated FirePoint");
                }
                
                // Get and set the Rigidbody
                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    p1Rb = rb;
                    Debug.Log("Found Player 1 Rigidbody");
                }
                else
                {
                    Debug.LogWarning("Player 1 has no Rigidbody component!");
                }
            }
            else if (player.playerNumber == 2)
            {
                // Set the transform/firepoint
                p2FirePoint = player.transform;
                
                // Try to find a dedicated fire point child first
                Transform firePointChild = player.transform.Find("FirePoint");
                if (firePointChild != null)
                {
                    p2FirePoint = firePointChild;
                    Debug.Log("Found Player 2 dedicated FirePoint");
                }
                
                // Get and set the Rigidbody
                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    p2Rb = rb;
                    Debug.Log("Found Player 2 Rigidbody");
                }
                else
                {
                    Debug.LogWarning("Player 2 has no Rigidbody component!");
                }
            }
        }
        
        // Log results
        Debug.Log($"Player1 found: {(p1FirePoint != null)}, Player2 found: {(p2FirePoint != null)}");
        
        // Check if references changed
        if (oldP1FirePoint != p1FirePoint)
            Debug.Log("Player 1 fire point reference updated");
        if (oldP2FirePoint != p2FirePoint)
            Debug.Log("Player 2 fire point reference updated");
        if (oldP1Rb != p1Rb)
            Debug.Log("Player 1 Rigidbody reference updated");
        if (oldP2Rb != p2Rb)
            Debug.Log("Player 2 Rigidbody reference updated");
    }

    void Update()
    {
        var gamepad1 = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
        var gamepad2 = Gamepad.all.Count > 1 ? Gamepad.all[1] : null;

        float m1 = gamepad1 != null ? gamepad1.rightTrigger.ReadValue() : p1Shoot.action.ReadValue<float>();
        float m2 = gamepad2 != null ? gamepad2.rightTrigger.ReadValue() : p2Shoot.action.ReadValue<float>();

        UpdateReload(ref p1ReloadTimer, ref p1Reloading, ref p1Ammo);
        UpdateReload(ref p2ReloadTimer, ref p2Reloading, ref p2Ammo);

        // Countdown timers
        if (p1Cooldown > 0f) p1Cooldown -= Time.deltaTime;
        if (p2Cooldown > 0f) p2Cooldown -= Time.deltaTime;

        HandleShooting(p1FirePoint, m1, ref p1Cooldown, ref p1Ammo, ref p1Reloading, ref p1ReloadTimer);
        HandleShooting(p2FirePoint, m2, ref p2Cooldown, ref p2Ammo, ref p2Reloading, ref p2ReloadTimer);
    }

    private void HandleShooting(Transform firePoint, float input, ref float cooldown, ref int ammo, ref bool reloading, ref float reloadTimer)
    {
        if (firePoint == null || reloading) return;

        if (input > 0.1f && cooldown <= 0f && ammo > 0)
        {
            cooldown = fireRate;
            ammo--;

            if (bulletShotEffect != null)
            {
                bulletShotEffect.Play();
            }

            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bulletInstance.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.AddForce(firePoint.forward * bulletSpeed, ForceMode.VelocityChange);
            }

            Destroy(bulletInstance, 3f);

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