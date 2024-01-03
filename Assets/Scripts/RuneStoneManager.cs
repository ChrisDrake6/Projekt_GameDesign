using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// This is responsible for creating and managing slots for runestones.
/// I do this by playing around the parent/child relationship and the use of a layout group.
/// </summary>
public class RuneStoneManager : MonoBehaviour
{
    public static RuneStoneManager Instance { get; private set; }

    public GameObject possibleSlotIndicator;
    public GameObject startRune;

    List<GameObject> indicators = new List<GameObject>();

    public RuneStoneManager()
    {
        Instance = this;
    }

    /// <summary>
    /// Gets called when a runestone is picked up
    /// Displays all possible drop locations for runestones by creating slots there
    /// </summary>
    public void DisplayPossibleSlots()
    {
        GameObject lastSlot = transform.GetChild(transform.childCount - 1).gameObject;
        float offsetDistance = lastSlot.GetComponent<RectTransform>().sizeDelta.y * lastSlot.transform.localScale.y;

        Vector3 targetPosition = lastSlot.transform.position;
        targetPosition.y -= offsetDistance;

        GameObject indicator = Instantiate(possibleSlotIndicator, targetPosition, possibleSlotIndicator.transform.rotation, lastSlot.transform.parent);
        indicators.Add(indicator);

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
    /// <param name="target"></param>
    public void HandleListEnter(GameObject target)
    {
        int childIndex = target.transform.GetSiblingIndex();
        GameObject formerChild = target.transform.parent.GetChild(childIndex - 1)?.gameObject;

        // If the runestone has already moved down to make way, instead of creating a new slot, switch places with the indicator over it.
        // This is necessary because there was a bug that this method wasn't called when i moved a slot form an indicator down to the just moved runeslot,
        // resulting in the indicator still existing although it shouldn't.
        if (formerChild != null && indicators.Contains(formerChild))
        {
            // Move target runestone up
            transform.SetSiblingIndex(childIndex - 1);

            // if there already is an indicator, remove it. No need for two slots at the same location
            GameObject nextChild = target.transform.parent.GetChild(childIndex - 1)?.gameObject;
            if(nextChild != null && indicators.Contains(nextChild))
            {
                indicators.Remove(nextChild);
                Destroy(nextChild);
            }
        }
        else
        {
            // Create a slot
            GameObject indicator = Instantiate(possibleSlotIndicator, target.transform.position, possibleSlotIndicator.transform.rotation, target.transform.parent);
            // Move target runestone down
            indicator.transform.SetSiblingIndex(childIndex);
            indicators.Add(indicator);
        }
    }

    /// <summary>
    /// Gets called when the drag over a indicator ends
    /// </summary>
    /// <param name="target"></param>
    public void HandleListExit(GameObject target)
    {
        int index = target.transform.GetSiblingIndex();
        if (index < gameObject.transform.childCount - 1)
        {
            //indicators.Remove(target);
            Destroy(target);
        }
    }
}
