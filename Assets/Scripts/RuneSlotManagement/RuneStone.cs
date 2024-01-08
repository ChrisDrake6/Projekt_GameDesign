using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class RuneStone : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public CanvasGroup canvasGroup;
    public bool isSlotted = false;

    bool wasClicked = false;

    Compiler compiler;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        compiler = Compiler.Instance;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isSlotted = false;
        if (compiler.processRunning)
        {
            return;
        }
        canvasGroup.blocksRaycasts = false;

        RuneStoneManager.Instance.DisplayPossibleSlots();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (compiler.processRunning)
        {
            return;
        }
        //rectTransform.anchoredPosition += eventData.delta;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (compiler.processRunning)
        {
            return;
        }
        canvasGroup.blocksRaycasts = true;

        // if it is not slotted, destroy it.
        if (!isSlotted)
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
        if (compiler.processRunning)
        {
            return;
        }
        if (!wasClicked)
        {
            wasClicked = true;

            int childindex = gameObject.transform.GetSiblingIndex();
            GameObject clone = Instantiate(gameObject, transform.parent);
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
            if (collision.gameObject.CompareTag("RuneStone") && isSlotted && other.wasClicked)
            {
                RuneStoneManager.Instance.HandleListEnter(gameObject);
            }
        }
    }
}
