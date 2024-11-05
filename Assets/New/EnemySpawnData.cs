using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;       // The enemy prefab to spawn
    public float spawnProbability;       // The probability weight for spawning this enemy
}
