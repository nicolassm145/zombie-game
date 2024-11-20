using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI _buttonText;

    [SerializeField] private Color hoverColor = Color.yellow; 
    [SerializeField] private Color normalColor = Color.white; 

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