using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer; 
    [SerializeField] Slider soundtrackSlider; 
    [SerializeField] Slider sfxSlider; 
   
    void Start()
    {
        // Inicializa os sliders com valores normalizados (linear 0.001 a 1)
        float musicVolume = Mathf.Pow(10, PlayerPrefs.GetFloat("MusicVolume", 0) / 20);
        float sfxVolume = Mathf.Pow(10, PlayerPrefs.GetFloat("SFXVolume", 0) / 20);

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
    }

    public void SetSFXVolume(float sliderValue)
    {
        // Converte o valor do slider para escala logarítmica
        float volumeInDecibels = Mathf.Log10(sliderValue) * 20;
        audioMixer.SetFloat("SFXVolume", volumeInDecibels);

        // Salva o valor
        PlayerPrefs.SetFloat("SFXVolume", volumeInDecibels);
    }
}