using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemySpawnData> enemySpawnDataList; // List of enemy spawn data
    public float spawnInterval = 2f;
    public float xSpawnRange = 7.5f;
    public int baseHealth = 3;
    public int healthIncreaseInterval = 5; // Increase health every X seconds
    public int maxHealth = 40; // Maximum health cap

    private float totalProbability;

    void Start()
    {
        // Calculate the total probability
        totalProbability = 0f;
        foreach (var data in enemySpawnDataList)
        {
            totalProbability += data.spawnProbability;
        }

        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Randomly select an enemy based on probabilities
        GameObject enemyPrefab = GetRandomEnemyPrefab();

        float randomX = Random.Range(-xSpawnRange, xSpawnRange);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0);
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Calculate and assign health based on game time
        int enemyHealth = CalculateEnemyHealth();

        // Get the Enemy component (or any component that inherits from Enemy)
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.SetHealth(enemyHealth); // Set health using the new method
        }
        else
        {
            Debug.LogError("Enemy script not found on instantiated enemy prefab: " + newEnemy.name);
        }
    }

    GameObject GetRandomEnemyPrefab()
    {
        float randomPoint = Random.value * totalProbability;
        float cumulativeProbability = 0f;

        foreach (var data in enemySpawnDataList)
        {
            cumulativeProbability += data.spawnProbability;
            if (randomPoint <= cumulativeProbability)
            {
                return data.enemyPrefab;
            }
        }

        // Fallback in case of rounding errors
        return enemySpawnDataList[enemySpawnDataList.Count - 1].enemyPrefab;
    }

    int CalculateEnemyHealth()
    {
        int intervalsPassed = Mathf.FloorToInt(GameManager.Instance.gameTime / healthIncreaseInterval);
        int increasedHealth = baseHealth + intervalsPassed;
        return Mathf.Min(increasedHealth, maxHealth);
    }
}
