using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float enemySpeed = 2f;
    public int maxHealth = 3;
    protected int currentHealth;
    protected HealthBar healthBar;
    public Sprite[] damageSprites; // Sprites for different damage levels
    protected SpriteRenderer spriteRenderer;
    protected float[] healthThresholds = { 0.75f, 0.5f, 0.25f, 0f };
    public int scoreValue = 10;

    public Sprite corpseSprite; // Assign the corpse sprite in the Inspector
    protected bool isDead = false; // Flag to check if the enemy is dead

    public AudioClip deathSound; // Assign in the Inspector
    protected AudioSource audioSource; // Changed to protected

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        // Initialize the sprite to full health sprite
        UpdateSprite();

        // Get the HealthBar component
        healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }

        // Initialize the AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!isDead)
        {
            MoveTowardsHero();
        }
    }

    protected void MoveTowardsHero()
    {
        GameObject closestHero = GetClosestHero();
        if (closestHero != null)
        {
            Vector3 direction = (closestHero.transform.position - transform.position).normalized;
            transform.Translate(direction * enemySpeed * Time.deltaTime, Space.World);
        }
    }

    public void SetHealth(int health)
    {
        maxHealth = health;
        currentHealth = health;
    }

    GameObject GetClosestHero()
    {
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
        if (heroes.Length == 0)
            return null;

        GameObject closestHero = null;
        float shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject hero in heroes)
        {
            float distance = Vector3.Distance(currentPosition, hero.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestHero = hero;
            }
        }

        return closestHero;
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead)
            return; // Don't take damage if already dead

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            if (healthBar != null)
            {
                healthBar.SetHealth(currentHealth, maxHealth);
            }
            UpdateSprite();
        }
    }

    protected virtual void Die()
    {
        isDead = true;

        // Add score
        ScoreManager.Instance.AddScore(scoreValue);

        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Change sprite to corpse sprite
        if (corpseSprite != null)
        {
            spriteRenderer.sprite = corpseSprite;
        }
        else
        {
            Debug.LogWarning("Corpse sprite not assigned for " + gameObject.name);
        }

        // Disable health bar
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }

        // Disable collider
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Stop movement
        enemySpeed = 0f;

        // Start coroutine to destroy after 5 seconds
        StartCoroutine(DestroyAfterDelay(5f));
    }

    protected IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    void UpdateSprite()
    {
        float healthPercentage = (float)currentHealth / maxHealth;

        for (int i = 0; i < healthThresholds.Length; i++)
        {
            if (healthPercentage >= healthThresholds[i])
            {
                if (i < damageSprites.Length)
                {
                    spriteRenderer.sprite = damageSprites[i];
                }
                break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return; // Ignore collisions if dead

        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Hero"))
        {
            HeroController hero = collision.GetComponent<HeroController>();
            if (hero != null)
            {
                hero.TakeDamage(1);
            }
            Die();
        }
    }
}
