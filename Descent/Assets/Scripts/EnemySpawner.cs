using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs; 
    
    [Tooltip("The base number of enemies per chunk on Level 1")]
    public int enemiesPerFiftyTiles = 1; 

    public void SpawnEnemies(List<Vector2> floorPositions)
    {
        int currentLevel = 1;
        if (GameManager.Instance != null)
        {
            currentLevel = GameManager.Instance.level;
        }

        int scaledEnemiesPerFiftyTiles = enemiesPerFiftyTiles + (currentLevel - 1);

        scaledEnemiesPerFiftyTiles = Mathf.Max(1, scaledEnemiesPerFiftyTiles);

        List<Vector2> availableTiles = new List<Vector2>(floorPositions);
        
        int targetSpawnCount = (floorPositions.Count / 300) * scaledEnemiesPerFiftyTiles;
        
        int actualSpawnCount = 0; 

        for (int i = 0; i < targetSpawnCount; i++)
        {
            if (availableTiles.Count == 0) break;

            int randomIndex = Random.Range(0, availableTiles.Count);
            Vector2 randomTile = availableTiles[randomIndex];
            availableTiles.RemoveAt(randomIndex);
            
            GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(randomEnemy, new Vector3(randomTile.x, randomTile.y, -0.1f), Quaternion.identity);
            
            actualSpawnCount++; 
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetTargetEnemyCount(actualSpawnCount);
        }
    }
}