using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs; 
    
    
    public int enemiesPerFiftyTiles = 1; 

    public void SpawnEnemies(List<Vector2> floorPositions)
    {
        // Track tiles to makes sure enemies don't spawn over each other
        List<Vector2> availableTiles = new List<Vector2>(floorPositions);

        // Prevent overcrowding.
        int spawnCount = (floorPositions.Count / 300) * enemiesPerFiftyTiles;

        for (int i = 0; i < spawnCount; i++)
        {
            // Stop spawning if we somehow run out of empty floor tiles
            if (availableTiles.Count == 0) break;

            // Pick a random tile from the available ones
            int randomIndex = Random.Range(0, availableTiles.Count);
            Vector2 randomTile = availableTiles[randomIndex];

            // Remove that tile from the list so no one else can spawn on top of it
            availableTiles.RemoveAt(randomIndex);
            
            // Pick a random enemy type from list
            GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Spawn the enemy
            Instantiate(randomEnemy, new Vector3(randomTile.x, randomTile.y, -0.1f), Quaternion.identity);
        }

        // After spawning everything, tell GameManager to update the kill goal
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CountEnemiesInLevel();
        }
    }
}