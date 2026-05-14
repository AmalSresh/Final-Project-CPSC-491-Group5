using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the popup message shown when the player earns a damage boost.
///
/// SETUP IN UNITY:
///   1. Create a Canvas in your gameplay scene if you don't have one already
///      (GameObject -> UI -> Canvas). Set Render Mode to "Screen Space - Overlay".
///   2. Inside the Canvas, create an empty GameObject named "DamagePopup".
///   3. Attach this script to it.
///   4. Add a TextMeshProUGUI component as a child of DamagePopup and assign it
///      to the "popupText" field in the Inspector.
///   5. Position the text wherever you want the popup to appear (e.g. center screen).
///   6. The popup will fade in, stay for a moment, then fade out automatically.
/// </summary>
public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Instance;

    [Header("UI")]
    public TextMeshProUGUI popupText;

    [Header("Timing")]
    public float fadeInTime  = 0.3f;
    public float displayTime = 1.5f;
    public float fadeOutTime = 0.5f;

    private Coroutine activeCoroutine;

    void Awake()
    {
        Instance = this;
        if (popupText != null)
            popupText.alpha = 0f;
    }

    /// <summary>Call this from anywhere to show a popup message.</summary>
    public static void Show(string message)
    {
        if (Instance == null)
        {
            Debug.LogWarning("DamagePopup: No instance found in scene. Add the DamagePopup GameObject to your gameplay scene.");
            return;
        }
        Instance.ShowPopup(message);
    }

    private void ShowPopup(string message)
    {
        if (popupText == null) return;

        popupText.text = message;

        // If a popup is already showing, cancel it and restart
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(AnimatePopup());
    }

    private IEnumerator AnimatePopup()
    {
        // Fade in
        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            popupText.alpha = Mathf.Clamp01(t / fadeInTime);
            yield return null;
        }
        popupText.alpha = 1f;

        // Hold
        yield return new WaitForSeconds(displayTime);

        // Fade out
        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            popupText.alpha = Mathf.Clamp01(1f - (t / fadeOutTime));
            yield return null;
        }
        popupText.alpha = 0f;
        activeCoroutine = null;
    }
}
