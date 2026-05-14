using UnityEngine;

/// <summary>
/// Marker component added to the music GameObject when it is made persistent.
/// Used by MainMenuUI to detect if background music is already playing
/// so it does not start a second copy on scene reload.
/// 
/// Setup: You do NOT need to attach this manually. MainMenuUI adds it automatically
/// at runtime when it first makes the music object persistent.
/// </summary>
public class MusicManager : MonoBehaviour
{
    // Intentionally empty — this is just a marker so FindFirstObjectByType can detect
    // whether a persistent music object already exists in the scene.
}
