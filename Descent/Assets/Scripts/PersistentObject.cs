using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    void Awake()
    {
        // This ensures the object survives scene loads and level generation
        DontDestroyOnLoad(gameObject);
    }
}