using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverControler : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField] AudioClip buttonClickClip; 
    [SerializeField] float sceneChangeDelay = 0.5f;
    
    [SerializeField] TextMeshProUGUI zombiesKilledText;
    [SerializeField] TextMeshProUGUI timeAliveText;
    [SerializeField] TextMeshProUGUI roundText;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>(); 
        
        if (_audioSource != null && !_audioSource.isPlaying)
        {
            _audioSource.loop = true; 
            _audioSource.Play();
        }
        
        int roundReached = PlayerPrefs.GetInt("RoundReached", 1);
        int zombiesKilled = PlayerPrefs.GetInt("ZombiesKilled", 0);
        float timeAlive = PlayerPrefs.GetFloat("TimeAlive", 0f);

        zombiesKilledText.text = $"Zumbis mortos: {zombiesKilled}";
        timeAliveText.text = $"Tempo vivo: {timeAlive:F1} segundos";
        roundText.text = $"Rounds: {roundReached}";
        
    }
    
    public void PlayButtonClickSound()
    {
        if (_audioSource != null && buttonClickClip != null)
        {
            _audioSource.PlayOneShot(buttonClickClip); 
        }
    }
    
    public void RestartGame()
    {
        if (_audioSource != null)
            _audioSource.Stop();
        PlayButtonClickSound(); 
        StartCoroutine(ChangeSceneWithDelay("MainScene"));
    }
    
    private IEnumerator ChangeSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(sceneChangeDelay); 
        SceneManager.LoadScene(sceneName); 
    }


    private IEnumerator QuitGameWithDelay()
    {
        _audioSource.Stop();
        yield return new WaitForSeconds(sceneChangeDelay); 
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; 
        #else
            Application.Quit(); // Para sair do jogo no build
        #endif
    }
    public void SairJogo()
    {
        PlayButtonClickSound(); 
        StartCoroutine(QuitGameWithDelay());
    }
    
}