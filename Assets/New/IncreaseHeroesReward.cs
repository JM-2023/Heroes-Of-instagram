using UnityEngine;
using TMPro;

public class IncreaseHeroesReward : MonoBehaviour
{
    public int rewardValue = 1; // Starting value
    public float moveSpeed = 2f; // Downward speed
    public float lifeTime = 20f; // Time before self-destruction
    private TextMeshPro textMesh;

    public AudioClip heroIncreaseSound; // Assign in the Inspector
    private AudioSource audioSource;

    private bool isCollected = false; // Flag to prevent multiple triggers

    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshPro>();
        UpdateDisplay();
        Destroy(gameObject, lifeTime); // Destroy after lifeTime seconds

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

    void UpdateDisplay()
    {
        if (textMesh != null)
        {
            textMesh.text = rewardValue.ToString();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected)
            return; // Ignore further collisions after collection

        if (collision.CompareTag("Hero"))
        {
            isCollected = true; // Set the flag to prevent multiple triggers

            // Play the hero increase sound
            if (heroIncreaseSound != null)
            {
                audioSource.PlayOneShot(heroIncreaseSound);
            }

            // Grant the reward
            TeamManager.Instance.AddHeroes(rewardValue);

            // Disable collider to prevent further collisions
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Disable visual elements
            if (textMesh != null)
            {
                textMesh.enabled = false;
            }
            // Disable sprite renderer
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }

            // Destroy game object after the sound has played
            Destroy(gameObject, heroIncreaseSound.length);
        }
    }
}
