using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RuneStone : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public CanvasGroup canvasGroup;
    public Transform defaultParent;

    RectTransform rectTransform;
    bool wasClicked = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(defaultParent);

        RuneStoneManager.Instance.DisplayPossibleSlots();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (transform.parent == defaultParent)
        {
            Destroy(gameObject);
        }
        RuneStoneManager.Instance.ResetDisplayOfPossibleSlots();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!wasClicked)
        {
            wasClicked = true;
            Instantiate(gameObject, rectTransform.parent);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RuneStone") && transform.parent != defaultParent)
        {
            RuneStoneManager.Instance.HandleListEnter(gameObject);
        }
    }
}
