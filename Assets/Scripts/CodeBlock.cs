using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CodeBlock : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public CanvasGroup canvasGroup;

    private RectTransform rectTransform;
    private Transform parentAtDragStart;

    protected bool wasMoved = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!wasMoved)
        {
            parentAtDragStart = gameObject.transform.parent;
            Instantiate(gameObject, parentAtDragStart);
            wasMoved = true;
        }
        else
        {
            gameObject.transform.parent = gameObject.transform.parent.parent;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6F;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1F;
        canvasGroup.blocksRaycasts = true;
        if (transform.parent == parentAtDragStart)
        {
            Destroy(gameObject);
        }
    }
}
