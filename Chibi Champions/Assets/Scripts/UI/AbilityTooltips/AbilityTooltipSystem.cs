using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTooltipSystem : MonoBehaviour
{
    private static AbilityTooltipSystem current;

    public AbilityTooltip tooltip;

    private void Awake()
    {
        current = this;
    }

    public static void Show(string content, string header, string control)
    {
        current.tooltip.SetText(content, header, control);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
