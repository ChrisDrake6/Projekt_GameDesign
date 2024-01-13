using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    public string content;
    public string header;


    //mouse over object? show/hide tooltip
    public void OnPointerEnter(PointerEventData eventData)
    {
        PopupTextSystem.Instance.ShowToolTip(content, header);
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        PopupTextSystem.Instance.HideToolTip();
    }
}
