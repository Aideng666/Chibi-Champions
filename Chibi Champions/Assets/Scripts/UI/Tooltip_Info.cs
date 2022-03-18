using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tooltip_Info : MonoBehaviour
{
    private static Tooltip_Info Instance;

    [SerializeField]
    private Camera uiCamera;
    [SerializeField]
    private RectTransform canvasRectTransform;

    private TMP_Text nameText;
    private RectTransform backgroundRectTransform;

    private void Awake()
    {
        Instance = this;
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        nameText = transform.Find("nameText").GetComponent<TMP_Text>();

        HideTooltip();
    }

    private void Update()
    {
        // Leave camera as null if using Screen Space Overlay
        // If using Screen Space Camera, provide ref camera 
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;

        Vector2 anchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;
        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y - backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height + backgroundRectTransform.rect.height;
        }
        transform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    }

    private void ShowTooltip(string itemName)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        nameText.text = itemName;
        Update();
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTootip_Static(string itemName)
    {
        Instance.ShowTooltip(itemName);
    }

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }
}
