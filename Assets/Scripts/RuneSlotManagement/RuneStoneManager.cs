using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.UIElements;

/// <summary>
/// This is responsible for creating and managing slots for runestones.
/// I do this by playing around the parent/child relationship and the use of a layout group.
/// </summary>
public class RuneStoneManager : MonoBehaviour
{
    public ScrollRect scroller;
    public float scollSnapToBottomDelay = 0.05F;
    public static RuneStoneManager Instance { get; private set; }

    public Transform runeStonesParent;
    //public GraphicRaycaster graphicRaycaster;
    public GameObject possibleSlotIndicator;

    List<GameObject> indicators = new List<GameObject>();

    public RuneStoneManager()
    {
        Instance = this;
    }

    /// <summary>
    /// Directly adds RuneStone on user input
    /// </summary>
    /// <param name="runeStone"></param>
    public void AddRuneStone(GameObject runeStone)
    {
        runeStone.transform.SetParent(runeStonesParent);
        Invoke("SnapScrollerToBottom", scollSnapToBottomDelay);

        SoundManager.Instance.PlaySound(SoundManager.Instance.crystalset);
    }

    /// <summary>
    /// Gets called when a runestone is picked up
    /// Displays all possible drop locations for runestones by creating slots there
    /// </summary>
    public void DisplayPossibleSlots()
    {
        // TODO: Deal with if and loop offsets
        GameObject indicator = Instantiate(possibleSlotIndicator, Vector3.zero, possibleSlotIndicator.transform.rotation, runeStonesParent);
        indicators.Add(indicator);
        Invoke("SnapScrollerToBottom", scollSnapToBottomDelay);
    }

    /// <summary>
    /// Gets called when a runestone gets dropped
    /// All Indicators get deleted
    /// </summary>
    public void ResetDisplayOfPossibleSlots()
    {
        foreach (GameObject indicator in indicators)
        {
            Destroy(indicator);
        }
        indicators.Clear();
    }

    /// <summary>
    /// Gets called when a runestone gets hovered over an already slotted runestone
    /// The other runestone makes way for the new runestone by creating a slot at its location and moving down or up
    /// </summary>
    /// <param name="runeStoneToBeMoved"></param>
    public void HandleListEnter(GameObject runeStoneToBeMoved, GameObject EnteringRuneStone)
    {
        if (EnteringRuneStone.transform.CompareTag("RuneStone") && !EnteringRuneStone.GetComponent<RuneStone>().isSlotted)
        {
            int childIndex = runeStoneToBeMoved.transform.GetSiblingIndex();
            Transform parent = runeStoneToBeMoved.transform.parent;

            // If the runestone has already moved down to make way, dont create a new slot.
            // This is necessary because there was a bug that this method wasn't called when i moved a slot form an indicator down to the just moved runeslot,
            // resulting in the indicator still existing although it shouldn't.
            if (!(childIndex > 0 && indicators.Contains(parent.GetChild(childIndex - 1).gameObject)))
            {
                // Create a slot
                GameObject indicator = Instantiate(possibleSlotIndicator, runeStoneToBeMoved.transform.position, possibleSlotIndicator.transform.rotation, runeStoneToBeMoved.transform.parent);
                // Move target runestone down
                indicator.transform.SetSiblingIndex(childIndex);
                indicators.Add(indicator);
            }
        }
    }

    /// <summary>
    /// Gets called when the drag over a indicator ends
    /// </summary>
    /// <param name="target"></param>
    public void HandleListExit(GameObject target)
    {
        int index = target.transform.GetSiblingIndex();
        if (index < runeStonesParent.childCount - 1)
        {
            //indicators.Remove(target);
            Destroy(target);
        }
    }

    public void Clear()
    {
        foreach (Transform child in runeStonesParent)
        {
            if (child.gameObject.CompareTag("RuneStone"))
            {
                Destroy(child.gameObject);
            }
            Compiler.Instance.processRunning = false;
        }
    }

    public void SnapScrollerToBottom()
    {
        scroller.verticalNormalizedPosition = 0;
    }
}
