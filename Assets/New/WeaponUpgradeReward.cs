using UnityEngine;

public class WeaponUpgradeReward : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    public float moveSpeed = 2f;
    public float lifeTime = 15f;

    private HealthBar healthBar;

    public AudioClip upgradeSound; // Assign in the Inspector
    private AudioSource audioSource;

    private bool isDestroyed = false; // Flag to prevent multiple triggers

    void Start()
    {
        currentHealth = maxHealth;
        Destroy(gameObject, lifeTime);

        // Get the HealthBar component
        healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }

        // Initialize the AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        MoveDownward();
    }

    void MoveDownward()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (isDestroyed)
            return; // Ignore if already in the process of being destroyed

        currentHealth -= damage;
        Debug.Log("Current Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            isDestroyed = true; // Set the flag to prevent further triggers

            // Play the upgrade sound
            if (upgradeSound != null)
            {
                audioSource.PlayOneShot(upgradeSound);
            }

            // Upgrade weapons
            TeamManager.Instance.UpgradeWeapons();

            // Disable collider to prevent further collisions
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Disable visual elements
            if (healthBar != null)
            {
                Destroy(healthBar.gameObject);
            }
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }

            // Destroy game object after the sound has played
            Destroy(gameObject, upgradeSound.length);
        }
        else
        {
            // Update the health bar
            if (healthBar != null)
            {
                Debug.Log("Updating Health Bar");
                healthBar.SetHealth(currentHealth, maxHealth);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroyed)
            return; // Ignore collisions if already being destroyed

        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(collision.GetComponent<Bullet>().damage);
            Destroy(collision.gameObject);
        }
    }
}
