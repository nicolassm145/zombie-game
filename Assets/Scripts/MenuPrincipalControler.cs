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
    
}