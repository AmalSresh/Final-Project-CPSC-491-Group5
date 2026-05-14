using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Enemy Tracking")]
    public int totalKills;
    public int levelKills;
    public int level;
    public int enemiesRequiredToWin;
    
    [Header("Level Generation Refs")]
    public DungeonGenerator dungeonGenerator;
    public EnemySpawner enemySpawner;
    public PlayerSpawner playerSpawner;
    public Transform player;

    [Header("Items")]
    public GameObject HealthPack;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddKill()
    {
        totalKills++;
        levelKills++;
        
        Debug.Log($"Level Kills: {levelKills} | Total Kills: {totalKills}");

        if (levelKills >= enemiesRequiredToWin)
        {
            Debug.Log("Level Objective Met!");
            // Trigger your win screen or open a door here
            // Implement more graceful handoff to next level
            StartLevel();
        }
    }


    public void StartLevel()
    {
        Debug.Log("Generating new level...");
        level++;
        // Find the pieces that exist in the newly loaded scene
        dungeonGenerator = Object.FindAnyObjectByType<DungeonGenerator>();
        enemySpawner = Object.FindAnyObjectByType<EnemySpawner>();
        playerSpawner = Object.FindAnyObjectByType<PlayerSpawner>();

        
        // If we loaded a scene without a dungeon (like returning to the menu), stop here.
        if (dungeonGenerator == null) 
        {
            Debug.Log("No Dungeon Generator found in this scene. Skipping generation.");
            return; 
        }

        // Build the Map
        dungeonGenerator.GenerateDungeon();

        List<Vector2> validTiles = dungeonGenerator.GetWalkableTiles();

        if (HealthPack != null && validTiles.Count > 0)
        {
            // Pick a random tile index
            int randomIndex = Random.Range(0, validTiles.Count);
            Vector2 randomTile = validTiles[randomIndex];

            // Spawn the health pack
            Instantiate(HealthPack, new Vector3(randomTile.x, randomTile.y, -0.1f), Quaternion.identity);

            // Optional: Remove that tile from the list so an enemy doesn't spawn exactly on top of it
            validTiles.RemoveAt(randomIndex);
        }

        // Spawn Enemies & Update Goal
        enemySpawner.SpawnEnemies(validTiles);
        //CountEnemiesInLevel();

        // Spawn Player
        if (validTiles.Count > 0)
        {
            // Check if player from last level is still alive/exists
            GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");

            if (existingPlayer == null)
            {
                // No player found - This must be the first level so spawn them
                if (playerSpawner != null)
                {
                    GameObject spawnedPlayer = playerSpawner.SpawnPlayerAt(validTiles[0]);
                    player = spawnedPlayer.transform; 

                    // Keep player available for next level to save stats and inventory
                    DontDestroyOnLoad(spawnedPlayer);
                }
                else
                {
                    Debug.LogError("Could not find a PlayerSpawner in the scene!");
                }
            }
            else
            {
                // If a player is found from the previous level, teleport them to the start of the map
                player = existingPlayer.transform;
                player.position = validTiles[0];
            }

            // setup camera
            if (Camera.main != null)
            {
                CameraFollow camScript = Camera.main.GetComponent<CameraFollow>();
                if (camScript != null)
                {
                    camScript.target = player;
                }

                Camera.main.transform.position = new Vector3(player.position.x, player.position.y, Camera.main.transform.position.z);
            }
        }
    }

    public void SetTargetEnemyCount(int exactCount)
    {
        enemiesRequiredToWin = exactCount;
        levelKills = 0; 
        Debug.Log("Enemies locked in for this level: " + enemiesRequiredToWin);
    }

    public void ResetGame()
    {
        Debug.Log("GameManager is restarting the game...");

        // Unpause the game
        Time.timeScale = 1f;

        totalKills = 0;
        levelKills = 0;
        level = 0;

        // Heal the persistent player so they don't spawn dead
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            PlayerHealth hpScript = playerObj.GetComponent<PlayerHealth>();
            if (hpScript != null)
            {
                hpScript.ResetHealth();
            }

            PlayerXP xpScript = playerObj.GetComponent<PlayerXP>();
            if (xpScript != null)
            {
                xpScript.ResetXP();
            }
        }

        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Future systems
    // inventory.Clear();
    // level = 1;
}
