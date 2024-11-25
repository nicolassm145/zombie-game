using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool _isPaused = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject[] uiElementsToDisable;

    void Start()
    {
        if (pauseMenuUI != null) // Força a sempre começar o jogo desligado.
            pauseMenuUI.SetActive(false);
    }
    
    public void OnPause()
    {
        if (_isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0; 
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            foreach (GameObject uiElement in uiElementsToDisable) // Desativa toda a UI
            {
                uiElement.SetActive(false);
            }
        }
    }

    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1; 
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            foreach (GameObject uiElement in uiElementsToDisable) // Ativa toda a UI
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
