using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool _isPaused = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject configMenuUI;
    [SerializeField] GameObject[] uiElementsToDisable;
    
    [SerializeField] AudioSource backgroundMusic; // Música de fundo
    [SerializeField] AudioClip pauseSound;        // Som ao pausar
    [SerializeField] AudioClip resumeSound;       // Som ao retomar
    
    AudioSource sfxPlayer;

    void Start()
    {
        if (pauseMenuUI != null) // Força a sempre começar o jogo desligado.
            pauseMenuUI.SetActive(false);
        
        if (configMenuUI != null) // Força a sempre começar o jogo desligado.
            configMenuUI.SetActive(false);
        
        sfxPlayer = gameObject.AddComponent<AudioSource>();

        // Inicia a música de fundo
        if (backgroundMusic != null)
            backgroundMusic.Play();
    }
    
    public void OnPause()
    {
        if (_isPaused)
        {
            if (configMenuUI.activeSelf)
                 BackToPauseUI();
            else
                ResumeGame();
        }
        else
            PauseGame();
        
    }

    public void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0; 
        
        if (backgroundMusic != null)
            backgroundMusic.Pause();
        
        PlaySoundEffect(pauseSound);
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
        
        if (backgroundMusic != null)
            backgroundMusic.UnPause(); // Retoma a música de fundo

        PlaySoundEffect(resumeSound);
        
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            foreach (GameObject uiElement in uiElementsToDisable) // Ativa toda a UI
            {
                uiElement.SetActive(true);
            }
            
        }
    }
    void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null && sfxPlayer != null)
        {
            sfxPlayer.PlayOneShot(clip);
        }
    }
    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void OpenConfigUI()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false); 

        if (configMenuUI != null)
            configMenuUI.SetActive(true); 
    }
    
    public void BackToPauseUI()
    {
        configMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}
