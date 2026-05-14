using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs; 
    public int enemiesPerFiftyTiles = 1; 

    public void SpawnEnemies(List<Vector2> floorPositions)
    {
        List<Vector2> availableTiles = new List<Vector2>(floorPositions);
        int targetSpawnCount = (floorPositions.Count / 300) * enemiesPerFiftyTiles;
        
        // NEW: Keep an exact tally of successful spawns
        int actualSpawnCount = 0; 

        for (int i = 0; i < targetSpawnCount; i++)
        {
            if (availableTiles.Count == 0) break;

            int randomIndex = Random.Range(0, availableTiles.Count);
            Vector2 randomTile = availableTiles[randomIndex];
            availableTiles.RemoveAt(randomIndex);
            
            GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(randomEnemy, new Vector3(randomTile.x, randomTile.y, -0.1f), Quaternion.identity);
            
            // Successfully spawned one!
            actualSpawnCount++; 
        }

        // Pass the EXACT foolproof number to the GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetTargetEnemyCount(actualSpawnCount);
        }
    }
}