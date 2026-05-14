using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;

    public static bool isPaused = false;

    void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
    
        if (Keyboard.current == null)
        {
            Debug.LogError("CRITICAL: Unity cannot find a Keyboard!");
            return;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("SUCCESS: Escape key was sensed!");

            if (GameOverManager.Instance != null && GameOverManager.Instance.gameObject.activeInHierarchy)
            {
                Debug.Log("BLOCKED: Game Over screen is showing.");
                return; 
            }

            if (isPaused)
            {
                Debug.Log("Action: Resuming Game");
                Resume();
            }
            else
            {
                Debug.Log("Action: Pausing Game");
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Destroy(playerObj);
        }

        SceneManager.LoadScene("TitleMenuScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting from Pause Menu...");
        Application.Quit();
    }
}