using UnityEngine;
using TMPro; // Required for TextMeshPro

public class HUDManager : MonoBehaviour
{
    [Header("UI Text Elements")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI enemiesLeftText;

    private PlayerXP playerXP; // Assuming you have a script named this

    void Update()
    {
        // 1. Pull data from the persistent GameManager
        if (GameManager.Instance != null)
        {
            killsText.text = "Total Kills: " + GameManager.Instance.totalKills;
            
            // Calculate how many enemies are left in the current level
            int remaining = GameManager.Instance.enemiesRequiredToWin - GameManager.Instance.levelKills;
            
            // Prevent it from showing negative numbers if you over-kill
            remaining = Mathf.Max(0, remaining); 
            
            enemiesLeftText.text = "Enemies Left: " + remaining;

            levelText.text = "Level: " + GameManager.Instance.level;
        }

        // 2. Find the persistent Player safely
        // Because the player might spawn a split-second after the UI, we check constantly
        if (playerXP == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerXP = playerObj.GetComponent<PlayerXP>();
            }
        }

        // 3. Pull data from the Player
        if (playerXP != null)
        {
            xpText.text = "XP: " + playerXP.currentXP;
        }
    }
}