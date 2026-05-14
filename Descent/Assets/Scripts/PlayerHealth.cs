using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public Slider healthBar;

    [Header("Audio")]
    [Tooltip("AudioSource whose clip is the player hurt sound. " +
             "Add an AudioSource to the Player prefab, assign your hurt .wav/.ogg as its clip, " +
             "Play On Awake = OFF, Loop = OFF.")]
    public AudioSource damageAudioSource;

    void Start()
    {
        // Load healthbar programmatically instead of through inspector
        if (healthBar == null)
        {
            GameObject foundSlider = GameObject.Find("HealthBar");
            if (foundSlider != null)
            {
                healthBar = foundSlider.GetComponent<Slider>();
            }
            else
            {
                Debug.LogError("Could not find HealthBar");
            }
        }

        // Auto-grab AudioSource if not assigned in Inspector
        if (damageAudioSource == null)
            damageAudioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.maxValue = maxHealth;

        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        // Play hurt sound
        if (damageAudioSource != null && damageAudioSource.clip != null)
            damageAudioSource.PlayOneShot(damageAudioSource.clip);

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

        // Force the visual UI bar to update immediately
        Start();
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
