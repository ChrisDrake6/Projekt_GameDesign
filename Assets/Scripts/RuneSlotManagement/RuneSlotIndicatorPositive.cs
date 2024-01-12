using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneSlotIndicatorPositive : MonoBehaviour, IDropHandler
{
    public Color defaultColor = Color.green;
    public Color hoverColor = Color.blue;
    
    bool justInitialized = true;
    Image imageComponent;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
        imageComponent.color = defaultColor;
    }

    //private void Update()
    //{
    //    if (transform.GetSiblingIndex() == transform.parent.childCount - 1 && justInitialized)
    //    {
    //        ScrollRect scrollRect = GameObject.Find("Scrollarea").GetComponent<ScrollRect>();
    //        scrollRect.verticalNormalizedPosition = 0;
    //        justInitialized = false;
    //    }
    //}

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            // If runestone gets dropped over the slot, set its position, parent and childindex to the slot
            eventData.pointerDrag.transform.SetParent(transform.parent, false);
            eventData.pointerDrag.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex());
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<RuneStone>().isSlotted = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RuneStone"))
        {
            imageComponent.color = hoverColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RuneStone"))
        {
            imageComponent.color = defaultColor;
            RuneStoneManager.Instance.HandleListExit(gameObject);
        }
    }
}