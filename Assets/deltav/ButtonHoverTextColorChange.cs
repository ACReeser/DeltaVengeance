using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverTextColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Text buttonText;
    private Color nonHoverColor;
    public Color HoverColor;

    void Start()
    {
        nonHoverColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = nonHoverColor;
    }
}

