using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RuneSlotIndicatorPositive : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            // If runestone gets dropped over the slot, set its position, parent and childindex to the slot
            eventData.pointerDrag.transform.SetParent(transform.parent, false);
            eventData.pointerDrag.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex());
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("RuneStone"))
        {
            RuneStoneManager.Instance.HandleListExit(gameObject);
        }
    }
}