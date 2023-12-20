using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;

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


    public void DisplayPossibleSlots()
    {
        GameObject lastSlot = transform.GetChild(transform.childCount-1).gameObject;
        float offsetDistance = lastSlot.GetComponent<RectTransform>().sizeDelta.y * lastSlot.transform.localScale.y;

        Vector3 targetPosition = lastSlot.transform.position;
        targetPosition.y -= offsetDistance;

        GameObject indicator = Instantiate(possibleSlotIndicator, targetPosition, possibleSlotIndicator.transform.rotation, lastSlot.transform.parent);
        indicators.Add(indicator);

    }

    public void ResetDisplayOfPossibleSlots()
    {
        foreach (GameObject indicator in indicators)
        {
            Destroy(indicator);
        }
        indicators.Clear();
    }

    public void HandleListEnter(GameObject target)
    {
        int childIndex = target.transform.GetSiblingIndex();

        GameObject indicator = Instantiate(possibleSlotIndicator, target.transform.position, possibleSlotIndicator.transform.rotation, target.transform.parent);
        indicator.transform.SetSiblingIndex(childIndex);
        indicators.Add(indicator);
    }

    public void HandleListExit(GameObject target) 
    { 
        int index = target.transform.GetSiblingIndex();
        if(index < gameObject.transform.childCount -1)
        {
            //indicators.Remove(target);
            Destroy(target);
        }
    }
}
