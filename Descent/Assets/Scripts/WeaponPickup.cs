using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Weapon Stats")]
    public string weaponName  = "Sword";
    public int damageBoost    = 2;
    public float rangeBoost   = 0.5f;

    [Header("Audio")]
    [Tooltip("AudioSource whose clip is the weapon pickup sound. " +
             "Add an AudioSource to the WeaponPickup prefab, assign your pickup .wav/.ogg, " +
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

        if (playerAttack != null)
        {
            playerAttack.IncreaseDamage(damageBoost);
            playerAttack.IncreaseRange(rangeBoost);

            // Play pickup sound before destroying.
            // Detach so it survives the Destroy call.
            if (pickupAudioSource != null && pickupAudioSource.clip != null)
            {
                pickupAudioSource.transform.SetParent(null);
                pickupAudioSource.PlayOneShot(pickupAudioSource.clip);
                Destroy(pickupAudioSource.gameObject, pickupAudioSource.clip.length + 0.1f);
            }

            Debug.Log("Picked up weapon: " + weaponName);
            Destroy(gameObject);
        }
    }
}
