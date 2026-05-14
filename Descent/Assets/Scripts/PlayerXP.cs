using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    public int currentXP = 0;

    [Header("Progression Settings")]
    [Tooltip("How much XP needed to earn a damage boost")]
    public int xpPerDamageBoost = 30;
    [Tooltip("How much damage is added each time")]
    public float damageBoostAmount = 0.5f;

    private int xpThreshold; // next XP milestone to hit
    private PlayerAttack playerAttack;

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        xpThreshold = xpPerDamageBoost;
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log("Player gained XP. Current XP: " + currentXP);

        // Check if we crossed a threshold
        while (currentXP >= xpThreshold)
        {
            ApplyDamageBoost();
            xpThreshold += xpPerDamageBoost;
        }
    }

    private void ApplyDamageBoost()
    {
        if (playerAttack != null)
        {
            playerAttack.damage += (int)damageBoostAmount;
            Debug.Log("Damage boosted! New damage: " + playerAttack.damage);
        }

        // Show popup
        DamagePopup.Show("+0.5 damage! Killing it out there");
    }

    public void ResetXP()
    {
        currentXP = 0;
        xpThreshold = xpPerDamageBoost;
        Debug.Log("XP has been reset");
    }
}
