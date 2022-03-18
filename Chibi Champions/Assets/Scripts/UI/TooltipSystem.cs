using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
