using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool mouse_over = false;

    public GameObject panel;
    private void Update()
    {
        if (mouse_over)
        {
            Debug.Log("Mouse Over");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        Debug.Log("Mouse Enter");
        panel.SetActive(true);
    }   
    
    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        Debug.Log("Mouse Exit");
        panel.SetActive(false);
    }
}