using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonHoverTextColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text buttonText;
    private Color nonHoverColor;
    public Color HoverColor;

    internal BuildableType AssociatedBuildable  = BuildableType.Unknown;

    void Start()
    {
        nonHoverColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = HoverColor;
        OnHover.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = nonHoverColor;
    }

    [SerializeField]
    public UnityEvent OnHover;
}

