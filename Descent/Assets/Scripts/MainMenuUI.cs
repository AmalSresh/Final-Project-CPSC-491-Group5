using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Background Music")]
    [Tooltip("AudioSource whose clip is your background music. " +
             "Set Loop = ON, Play On Awake = OFF on this AudioSource.")]
    public AudioSource musicSource;

    // Tag used to prevent duplicate music objects after scene reloads
    private const string MusicTag = "PersistentMusic";

    void Start()
    {
        // Apply saved volume to mixer
        float volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        if (audioMixer != null)
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

        StartMusic();
    }

    private void StartMusic()
    {
        if (musicSource == null) return;

        // Check if persistent music is already playing from a previous visit
        // to avoid stacking duplicate music objects
        GameObject existing = GameObject.FindGameObjectWithTag(MusicTag);
        if (existing != null)
        {
            // Music is already alive from a prior scene load — don't start a second copy
            Destroy(musicSource.gameObject);
            return;
        }

        // Tag and persist this music object so it survives scene loads
        musicSource.gameObject.tag = MusicTag;
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
