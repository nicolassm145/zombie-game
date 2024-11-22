using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;

    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject[] uiElementsToDisable;

    void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }
    
    
    public void OnPause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; 
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            foreach (GameObject uiElement in uiElementsToDisable)
            {
                uiElement.SetActive(false);
            }
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; 
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            foreach (GameObject uiElement in uiElementsToDisable)
            {
                uiElement.SetActive(true);
            }
            
        }
    }
    
    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuPrincipal");
    }
    
}
