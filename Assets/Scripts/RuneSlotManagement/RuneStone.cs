using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class RuneStone : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public CanvasGroup canvasGroup;

    Transform defaultParent;
    RectTransform rectTransform;

    bool wasClicked = false;

    void Awake()
    {
        defaultParent = transform.parent;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        RuneStoneManager.Instance.DisplayPossibleSlots();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //rectTransform.anchoredPosition += eventData.delta;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // if it is not slotted, destroy it.
        if (transform.parent == defaultParent.parent)
        {
            Destroy(gameObject);
        }
        RuneStoneManager.Instance.ResetDisplayOfPossibleSlots();
    }

    /// <summary>
    /// create a clone of the runeslot as new pickup.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!wasClicked)
        {
            wasClicked = true;

            int childindex = gameObject.transform.GetSiblingIndex();
            GameObject clone = Instantiate(gameObject, defaultParent);
            clone.transform.SetSiblingIndex(childindex);
        }
        transform.SetParent(transform.parent.parent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Gets triggered when the runestone gets hovered of an already slotted runestone
        RuneStone other = collision.gameObject.GetComponent<RuneStone>();
        if (other != null)
        {
            if (collision.gameObject.CompareTag("RuneStone") && transform.parent != defaultParent && other.wasClicked)
            {
                RuneStoneManager.Instance.HandleListEnter(gameObject);
            }
        }
    }
}
