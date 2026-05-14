using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Background Music")]
    public AudioSource musicSource;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        if (audioMixer != null)
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

        StartMusic();
    }

    private void StartMusic()
    {
        if (musicSource == null) return;

        // Check if a MusicManager already exists from a previous scene load
        MusicManager existing = Object.FindFirstObjectByType<MusicManager>();
        if (existing != null)
        {
            // Already playing — destroy the duplicate source on this object
            Destroy(musicSource.gameObject);
            return;
        }

        // Move the music source to a dedicated persistent manager
        musicSource.gameObject.AddComponent<MusicManager>();
        DontDestroyOnLoad(musicSource.gameObject);

        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    public void StartGame()
    {
        Debug.Log("NEW GAME STARTED");

        if (GameManager.Instance != null)
            GameManager.Instance.ResetGame();

        SceneManager.LoadScene("Test_room");
    }

    public void OpenOptions()
    {
        Debug.Log("OPTIONS CLICKED");
        SceneManager.LoadScene("OptionsMenu");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT CLICKED");

        if (Application.isEditor)
        {
            Debug.Log("Application.Quit() ignored in Editor.");
            return;
        }

        Application.Quit();
    }
}
