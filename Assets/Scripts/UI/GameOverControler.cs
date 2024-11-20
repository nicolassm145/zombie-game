using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverControler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI zombiesKilledText;
    [SerializeField] TextMeshProUGUI timeAliveText;

    void Start()
    {
        int zombiesKilled = PlayerPrefs.GetInt("ZombiesKilled", 0);
        float timeAlive = PlayerPrefs.GetFloat("TimeAlive", 0f);

        zombiesKilledText.text = $"Zumbis mortos: {zombiesKilled}";
        timeAliveText.text = $"Tempo vivo: {timeAlive:F1} segundos";
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    public void SairJogo()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Para parar o jogo no editor
        #else
                Application.Quit(); // Para sair do jogo no build
        #endif
    }
    
}