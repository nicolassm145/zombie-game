using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalController : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField] AudioClip buttonClickClip; // Referência ao som de clique
    [SerializeField] float sceneChangeDelay = 0.5f; // Delay para troca de cena

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>(); // Obtém o AudioSource no mesmo GameObject

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
            _audioSource.PlayOneShot(buttonClickClip); // Reproduz o som de clique
        }
    }

    public void IniciarJogo()
    {
        if (_audioSource != null)
            _audioSource.Stop();
        PlayButtonClickSound(); // Toca o som ao clicar
        StartCoroutine(ChangeSceneWithDelay("MainScene"));
    }

    public void SairJogo()
    {
        PlayButtonClickSound(); 
        StartCoroutine(QuitGameWithDelay());
    }

    // Corrotina para mudar de cena com delay
    private IEnumerator ChangeSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(sceneChangeDelay); 
        SceneManager.LoadScene(sceneName); 
    }

    // Corrotina para sair do jogo com delay
    private IEnumerator QuitGameWithDelay()
    {
        _audioSource.Stop();
        yield return new WaitForSeconds(sceneChangeDelay); // Espera o tempo de delay
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Para parar o jogo no editor
        #else
            Application.Quit(); // Para sair do jogo no build
        #endif
    }
}
