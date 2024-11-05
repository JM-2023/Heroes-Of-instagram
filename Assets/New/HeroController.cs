using UnityEngine;

public class HeroController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float xLimit = 7.5f;
    public int health = 3;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    // Weapon Upgrade Variables
    public Sprite[] weaponSprites; // Array of weapon sprites
    public float[] fireRates; // Corresponding fire rates
    private int weaponLevel = 0; // Current weapon level
    private SpriteRenderer spriteRenderer;

    private AudioSource audioSource;
    public AudioClip bulletSound; // Assign in the Inspector

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateWeapon();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Move();
        Shoot();
    }

    void Move()
    {
        float moveDirection = Input.GetAxisRaw("Horizontal");
        Vector3 newPosition = transform.position + Vector3.right * moveDirection * moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -xLimit, xLimit);
        transform.position = newPosition;
    }

    void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            AudioManager.Instance.PlaySound(bulletSound);
            nextFireTime = Time.time + fireRate;
        }
    }


    public void UpgradeWeapon()
    {
        weaponLevel = Mathf.Min(weaponLevel + 1, weaponSprites.Length - 1);
        UpdateWeapon();
    }

    void UpdateWeapon()
    {
        spriteRenderer.sprite = weaponSprites[weaponLevel];
        fireRate = fireRates[weaponLevel];
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            TeamManager.Instance.RemoveHero(gameObject);
            Destroy(gameObject);
        }
    }
}
