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

    private Image image;
    private TMP_Text nameText;
    private TMP_Text descriptionText;
    private TMP_Text costText;
    private RectTransform backgroundRectTransform;

    private void Awake()
    {
        Instance = this;
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        image = transform.Find("UPGRADE").GetComponent<Image>();
        nameText = transform.Find("NameText").GetComponent<TMP_Text>();
        descriptionText = transform.Find("DescriptionText").GetComponent<TMP_Text>();
        costText = transform.Find("Number").GetComponent<TMP_Text>();

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

    private void ShowTooltip(Sprite itemSprite, string itemName, string itemDescription, int cost)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        nameText.text = itemName;
        descriptionText.text = itemDescription;
        costText.text = cost.ToString();
        image.sprite = itemSprite;
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTootip_Static(Sprite itemSprite, string itemName, string itemDescription, int cost)
    {
        Instance.ShowTooltip(itemSprite, itemName, itemDescription, cost);
    }

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }
}
