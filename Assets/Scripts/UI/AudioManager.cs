using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer; 
    [SerializeField] Slider soundtrackSlider; 
    [SerializeField] Slider sfxSlider; 
    
    [SerializeField] TextMeshProUGUI musicPercentageText;
    [SerializeField] TextMeshProUGUI sfxPercentageText;
    
    void Start()
    {
        float musicVolume = Mathf.Pow(10, PlayerPrefs.GetFloat("MusicVolume", 1) / 20);
        float sfxVolume = Mathf.Pow(10, PlayerPrefs.GetFloat("SFXVolume", 1) / 20);
        
        musicPercentageText.text = Mathf.RoundToInt(musicVolume * 100) + "%";
        sfxPercentageText.text = Mathf.RoundToInt(sfxVolume * 100) + "%";
        soundtrackSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        // Adiciona listeners
        soundtrackSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    
    public void SetMusicVolume(float sliderValue)
    {
        // Converte o valor do slider para escala logarítmica
        float volumeInDecibels = Mathf.Log10(sliderValue) * 20;
        audioMixer.SetFloat("MusicVolume", volumeInDecibels);

        // Salva o valor
        PlayerPrefs.SetFloat("MusicVolume", volumeInDecibels);
        musicPercentageText.text = Mathf.RoundToInt(sliderValue * 100) + "%";
    }
   

    public void SetSFXVolume(float sliderValue)
    {
        // Converte o valor do slider para escala logarítmica
        float volumeInDecibels = Mathf.Log10(sliderValue) * 20;
        audioMixer.SetFloat("SFXVolume", volumeInDecibels);
        
        // Salva o valor
        PlayerPrefs.SetFloat("SFXVolume", volumeInDecibels);
        sfxPercentageText.text = Mathf.RoundToInt(sliderValue * 100) + "%";
    }
}