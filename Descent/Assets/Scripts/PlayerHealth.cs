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
        // load healthbar programmatically instead of through inspector
        if (healthBar == null)
        {
            GameObject foundSlider = GameObject.Find("HealthBar");
            if (foundSlider != null)
            {
                healthBar = foundSlider.GetComponent<Slider>();
            }
            else{
                Debug.LogError("Could not find HealthBar");
            }
        }
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
        }

        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        // if (damageAudio != null)
        // {
        //     damageAudio.play();
        // }
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        Debug.Log("Current Health: " + currentHealth);

        if (currentHealth == 0)
        {
            Die();
            //GameOverManager.Instance.Show();
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
        Debug.Log("This is being called");
        if (healthBar != null)
        {   
            Debug.Log("Healthbar exists!");
            healthBar.value = currentHealth;
        }
    }

    public void ResetHealth()
    {
        currentHealth = 10; 
        
        // Force the visual UI bar to update immediately!
        // (Make sure to use your actual UI variable name here, like healthSlider, healthBar, etc.)
        Start();
        Debug.Log("Player Health and UI Reset to Full.");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 2. Unsubscribe if the player is destroyed (prevents memory leaks)
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 3. This runs automatically EVERY time a new scene finishes loading!
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("New scene loaded! Player is looking for the new Healthbar...");
        ResetHealth();
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