using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    // A shared memory bank of all the object names we have made persistent
    private static HashSet<string> savedObjectNames = new HashSet<string>();

    void Awake()
    {
        //Check if we already have an indestructible object with this exact name
        if (savedObjectNames.Contains(gameObject.name))
        {
            // This is a clone from a reloaded scene. Destroy it instantly
            Destroy(gameObject);
            return;
        }
        // Add its name to the memory bank and make it indestructible.
        savedObjectNames.Add(gameObject.name);
        DontDestroyOnLoad(gameObject);
    }
}