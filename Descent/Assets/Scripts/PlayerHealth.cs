using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public Slider healthBar;
   
    void Start()
    {
        currentHealth = maxHealth;
        ReconnectAndSyncUI();
    }

    public void ReconnectAndSyncUI()
    {
        GameObject foundSlider = GameObject.Find("HealthBar");
        if (foundSlider != null)
        {
            healthBar = foundSlider.GetComponent<Slider>();
            healthBar.maxValue = maxHealth;
        }
        else
        {
            Debug.LogWarning("Could not find HealthBar in this scene.");
        }

        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        Debug.Log("Current Health: " + currentHealth);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        Debug.Log("UpdateHealthBar is being called");
        if (healthBar != null)
        {   
            Debug.Log("Healthbar exists! Updating value.");
            healthBar.value = currentHealth;
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth; 
        
        ReconnectAndSyncUI();
        Debug.Log("Player Health and UI Reset to Full.");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("New scene loaded! Player is looking for the new Healthbar...");
        
        ReconnectAndSyncUI();
    }

    void Die()
    {
        Debug.Log("Player Died");

        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.Show();
        }
        else
        {
            Debug.LogError("Player died, but GameOverManager is missing from the scene!");
        }
    }
}