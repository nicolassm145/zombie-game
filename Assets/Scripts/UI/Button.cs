using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI _buttonText;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color hoverColor = Color.yellow; 

    void Awake()
    { 
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    { 
        _buttonText.color = normalColor;
    }
}