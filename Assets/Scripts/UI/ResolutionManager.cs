using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    void Start()
    {
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
        
        if (screenWidth < 1920 || screenHeight < 1080) 
        {
            Screen.SetResolution(1280, 720, true); 
        }
        else {
            Screen.SetResolution(1920, 1080, true); 
        }
    }
}