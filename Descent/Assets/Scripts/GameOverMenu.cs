using UnityEngine;
using UnityEngine.SceneManagement;
// Not in use anymore 5/12/2026
public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverUI;

    public void RestartGame()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Restart is pressed");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleMenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}