using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    private static Tooltip Instance;

    [SerializeField]
    private Camera uiCamera;
    [SerializeField]
    private RectTransform canvasRectTransform;

    private TMP_Text tooltipText;
    private RectTransform backgroundRectTransform;

    private void Awake()
    {
        Instance = this;
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        tooltipText = transform.Find("Text").GetComponent<TMP_Text>();

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

    private void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        tooltipText.text = tooltipString;
        Update();
        //float textPaddingSize = 4f;
        //Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2f, tooltipText.preferredHeight + textPaddingSize * 2f);
        //backgroundRectTransform.sizeDelta = backgroundSize;
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTootip_Static(string tooltipString)
    {
        Instance.ShowTooltip(tooltipString);
    }

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }
}
