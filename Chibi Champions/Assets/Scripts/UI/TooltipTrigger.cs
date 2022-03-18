using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Checks whether to call the show or hide functions if the mouse
// is hovered over the object

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int towerIndex;

    public string header;
    public string content;
    public string cost;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(content, header, cost);
        Debug.Log("Tower Index:" + towerIndex.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }
}
