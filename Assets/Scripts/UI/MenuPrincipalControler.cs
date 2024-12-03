using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalController : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField] AudioClip buttonClickClip; 
    [SerializeField] float sceneChangeDelay = 0.5f; 

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>(); 

        if (_audioSource != null && !_audioSource.isPlaying)
        {
            _audioSource.loop = true; 
            _audioSource.Play();
        }
    }

    public void PlayButtonClickSound()
    {
        if (_audioSource != null && buttonClickClip != null)
        {
            _audioSource.PlayOneShot(buttonClickClip); 
        }
    }

    public void IniciarJogo()
    {
        if (_audioSource != null)
            _audioSource.Stop();
        PlayButtonClickSound(); 
        StartCoroutine(ChangeSceneWithDelay("MainScene"));
    }

    public void SairJogo()
    {
        PlayButtonClickSound(); 
        StartCoroutine(QuitGameWithDelay());
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
}
