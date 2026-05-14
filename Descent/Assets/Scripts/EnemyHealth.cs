using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    public Slider healthBar;

    private Animator anim;
    private AudioSource audioSource;

    private bool isDead = false;

    void Start()
    {
        int currentLevel = 1;
        if (GameManager.Instance != null)
        {
            currentLevel = GameManager.Instance.level;
        }

        maxHealth = maxHealth + (currentLevel - 1);
        
        maxHealth = Mathf.Max(1, maxHealth);

        currentHealth = maxHealth;

        // Animate and play sounds based on enemy states
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        if (isDead == true)
        {
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hit");
        }
        else if (currentHealth == 0)
        {
            audioSource.Play();
            anim.SetTrigger("Dead");
            Die();
        }
    }

    void UpdateHealthBar()
    {
        healthBar.value = (float)currentHealth / maxHealth;
    }

    void Die()
    {
        isDead = true;

        Debug.Log(gameObject.name + " died");

        GetComponent<EnemyAI>().enabled = false;

        Collider2D myCollider = GetComponent<Collider2D>();
        if (myCollider != null)
        {
            myCollider.enabled = false;
        }

        PlayerXP xp = FindAnyObjectByType<PlayerXP>();
        if (xp != null)
        {
            xp.AddXP(5);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddKill();
        }
        
        Destroy(gameObject, audioSource.clip.length + 0.1f);
    }
}