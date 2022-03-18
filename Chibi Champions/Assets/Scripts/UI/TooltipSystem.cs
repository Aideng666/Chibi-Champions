using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Uses a singleton to keep one instance of the tooltip and to show 
// or hide the tooltip on the canvas

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;

    public TooltipScript tooltip;

    public void Awake()
    {
        current = this;
    }

    public static void Show(string content, string header = "", string cost = "")
    {
        current.tooltip.SetText(content, header, cost);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
