using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI _buttonText;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color hoverColor = Color.yellow;


    [SerializeField] AudioClip buttonHover;


    AudioSource _audioSource;

    void Awake()
    {
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttonText.color = hoverColor;
        _audioSource.PlayOneShot(buttonHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _buttonText.color = normalColor;
    }
}
   
  