using UnityEngine;

public class RewardSpawner : MonoBehaviour
{
    public GameObject increaseHeroesRewardPrefab;
    public GameObject weaponUpgradeRewardPrefab;
    public float spawnInterval = 10f;
    public float xSpawnRange = 7.5f;
    public float ySpawnPosition = 6f;

    void Start()
    {
        InvokeRepeating("SpawnRandomReward", 5f, spawnInterval);
    }

    void SpawnRandomReward()
    {
        int rewardType = Random.Range(0, 2); // 0 or 1
        float randomX = Random.Range(-xSpawnRange, xSpawnRange);
        Vector3 spawnPosition = new Vector3(randomX, ySpawnPosition, 0);

        if (rewardType == 0)
        {
            Instantiate(increaseHeroesRewardPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Instantiate(weaponUpgradeRewardPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
