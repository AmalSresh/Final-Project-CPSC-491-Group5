using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    public int currentXP = 0;

    [Header("Progression Settings")]
    [Tooltip("How much XP needed to earn a damage boost")]
    public int xpPerDamageBoost = 30;
    [Tooltip("How much damage is added each time")]
    public float damageBoostAmount = 0.5f;

    private int xpThreshold;
    private float accumulatedDamage = 0f; // tracks fractional damage
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
            accumulatedDamage += damageBoostAmount;

            // Only apply to int damage once we've accumulated a full point
            // e.g. two boosts of 0.5 = 1 full damage point added
            int damageToAdd = Mathf.FloorToInt(accumulatedDamage);
            if (damageToAdd > 0)
            {
                playerAttack.damage += damageToAdd;
                accumulatedDamage -= damageToAdd;
                Debug.Log("Damage boosted! New damage: " + playerAttack.damage);
            }
            else
            {
                Debug.Log("Damage accumulating: " + accumulatedDamage + " (need another boost for full point)");
            }
        }

        DamagePopup.Show("+0.5 damage! Killing it out there");
    }

    public void ResetXP()
    {
        currentXP = 0;
        xpThreshold = xpPerDamageBoost;
        accumulatedDamage = 0f;
        Debug.Log("XP has been reset");
    }
}
