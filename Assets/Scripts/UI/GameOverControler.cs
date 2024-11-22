using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverControler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI zombiesKilledText;
    [SerializeField] TextMeshProUGUI timeAliveText;
    [SerializeField] TextMeshProUGUI roundText;

    void Start()
    {
        int roundReached = PlayerPrefs.GetInt("RoundReached", 1);
        int zombiesKilled = PlayerPrefs.GetInt("ZombiesKilled", 0);
        float timeAlive = PlayerPrefs.GetFloat("TimeAlive", 0f);

        zombiesKilledText.text = $"Zumbis mortos: {zombiesKilled}";
        timeAliveText.text = $"Tempo vivo: {timeAlive:F1} segundos";
        roundText.text = $"Rounds: {roundReached}";
        
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