using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f); // Aumenta o botão
        ColorBlock cb = button.colors;
        cb.normalColor = Color.cyan; // Muda a cor do botão
        button.colors = cb;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.transform.localScale = new Vector3(1f, 1f, 1f); // Restaura o tamanho original
        ColorBlock cb = button.colors;
        cb.normalColor = Color.white; // Restaura a cor original
        button.colors = cb;
    }
}