using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float gameTime = 0f;
    public GameObject gameOverUI;
    public TextMeshProUGUI finalScoreText;

    // **Add these variables**
    public AudioClip gameOverSound; // Assign in the Inspector
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // **Initialize the AudioSource**
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        // **Play the game over sound**
        if (gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }

        // Activate the Game Over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);

            // Update the final score text
            if (finalScoreText != null)
            {
                int finalScore = ScoreManager.Instance.GetScore();
                finalScoreText.text = "Final Score: " + finalScore.ToString();
            }
        }
    }

    void Update()
    {
        gameTime += Time.deltaTime;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
