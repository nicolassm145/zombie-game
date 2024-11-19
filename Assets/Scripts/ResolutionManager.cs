using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    void Start()
    {
        // Detecta a resolução atual do monitor
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;

        // Define a resolução do jogo baseada na resolução do monitor
        if (screenWidth < 1920 || screenHeight < 1080)
        {
            // Ajusta para HD (1280x720) se o monitor não suportar Full HD
            Screen.SetResolution(1280, 720, true); 
        }
        else
        {
            // Mantém o Full HD (1920x1080) se suportado
            Screen.SetResolution(1920, 1080, true); 
        }
    }
}