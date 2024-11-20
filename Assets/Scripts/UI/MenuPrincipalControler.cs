using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalController : MonoBehaviour
{

    public void IniciarJogo()
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