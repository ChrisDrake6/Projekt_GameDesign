using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public Tooltip tooltip;

    public void Awake()
    {
        current = this;
    }

    //need content to show
    public static void Show (string content, string header="")
    {
        current.tooltip.SetText(content, header);

        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current .tooltip.gameObject.SetActive(false);
    }
}