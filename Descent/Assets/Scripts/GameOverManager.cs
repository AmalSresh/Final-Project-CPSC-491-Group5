using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false); 
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void OnRestartButtonClicked()
    {
        gameObject.SetActive(false); 

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; 
        gameObject.SetActive(false); 

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Destroy(playerObj);
        }

        SceneManager.LoadScene("TitleMenuScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}