using UnityEngine;

public class LevelStartup : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartLevel();
        }
    }
}