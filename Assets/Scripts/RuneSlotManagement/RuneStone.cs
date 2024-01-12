using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneStone : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public CanvasGroup canvasGroup;
    public bool isSlotted = false;

    public KeyCode shortcutDirectlyAddRuneStone;

    bool wasClicked = false;

    Compiler compiler;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        compiler = Compiler.Instance;
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

        if (Input.GetKey(shortcutDirectlyAddRuneStone))
        {
            Clone();
            wasClicked = true;
            isSlotted = true;
            RuneStoneManager.Instance.AddRuneStone(gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (compiler.processRunning)
        {
            return;
        }

        if (!wasClicked)
        {
            Clone();
            wasClicked = true;
        }
        GameObject canvas = GameObject.Find("Canvas");
        transform.SetParent(canvas.transform);
        isSlotted = false;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Gets triggered when the runestone gets hovered of an already slotted runestone
        RuneStone other = collision.gameObject.GetComponent<RuneStone>();
        if (other != null)
        {
            if (collision.gameObject.CompareTag("RuneStone") && isSlotted && other.wasClicked)
            {
                RuneStoneManager.Instance.HandleListEnter(gameObject, collision.gameObject);
            }
        }
    }

    /// <summary>
    /// Create Clone of RuneStone to be picked up next
    /// </summary>
    public void Clone()
    {
        int childindex = gameObject.transform.GetSiblingIndex();
        GameObject clone = Instantiate(gameObject, transform.parent);
        clone.transform.SetSiblingIndex(childindex);
    }
}
