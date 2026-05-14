using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [Tooltip("How much health to restore")]
    public int healAmount = 3;

    [Header("Audio")]
    [Tooltip("The sound effect to play when picked up")]
    public AudioClip pickupSound; 

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                if (playerHealth.currentHealth < playerHealth.maxHealth)
                {
                    playerHealth.Heal(healAmount);
                    
                    if (pickupSound != null)
                    {
                        // PlayClipAtPoint creates a temporary audio source that survives the health pack's destruction
                        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
}