using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    public enum PowerUpType
    {
        DamageBoost,
        RangeBoost,
        Heal
    }

    [Header("Power-Up Settings")]
    public PowerUpType powerUpType;
    public int damageBoostAmount   = 1;
    public float rangeBoostAmount  = 1f;
    public int healAmount          = 3;

    [Header("Audio")]
    [Tooltip("AudioSource whose clip is the pickup sound. " +
             "Add an AudioSource to the PowerUp prefab, assign your pickup .wav/.ogg, " +
             "Play On Awake = OFF, Loop = OFF.")]
    public AudioSource pickupAudioSource;

    void Awake()
    {
        if (pickupAudioSource == null)
            pickupAudioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        switch (powerUpType)
        {
            case PowerUpType.DamageBoost:
                if (playerAttack != null)
                {
                    playerAttack.IncreaseDamage(damageBoostAmount);
                    Debug.Log("Picked up Damage Boost!");
                }
                break;

            case PowerUpType.RangeBoost:
                if (playerAttack != null)
                {
                    playerAttack.IncreaseRange(rangeBoostAmount);
                    Debug.Log("Picked up Range Boost!");
                }
                break;

            case PowerUpType.Heal:
                if (playerHealth != null)
                {
                    playerHealth.Heal(healAmount);
                    Debug.Log("Picked up Health Power-Up!");
                }
                break;
        }

        // Play pickup sound before destroying the object.
        // We detach the AudioSource so it can finish playing after the GameObject is gone.
        if (pickupAudioSource != null && pickupAudioSource.clip != null)
        {
            pickupAudioSource.transform.SetParent(null);
            pickupAudioSource.PlayOneShot(pickupAudioSource.clip);
            Destroy(pickupAudioSource.gameObject, pickupAudioSource.clip.length + 0.1f);
        }

        Destroy(gameObject);
    }
}
