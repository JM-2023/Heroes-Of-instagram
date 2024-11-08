using UnityEngine;

public class BigEnemy : Enemy
{
    public int bigEnemyScoreValue = 30;

    protected override void Start()
    {
        base.Start();
        scoreValue = bigEnemyScoreValue; // Set the score value for BigEnemy
    }

    protected override void Die()
    {
        isDead = true;

        // Add score
        ScoreManager.Instance.AddScore(scoreValue);

        // **Play death sound**
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

        // Additional BigEnemy-specific death behavior can be added here
    }
}
