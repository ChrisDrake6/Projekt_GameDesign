using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public Image tooltip;

    public void Awake()
    {
        current = this;
    }
    public static void Show()
    {
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current .tooltip.gameObject.SetActive(false);
    }
}
