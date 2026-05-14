using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    
    public GameObject SpawnPlayerAt(Vector2 spawnPosition)
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab is missing in the Inspector!");
            return null;
        }

        // Spawn the player at the exact coordinate provided
        GameObject newPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        return newPlayer;
    }
}