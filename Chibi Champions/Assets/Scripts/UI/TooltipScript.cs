using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipScript : MonoBehaviour
{
    //public TMP_Text headerField;
    //public TMP_Text contentField;
    //public TMP_Text costField;

    //public LayoutElement layoutElement;

    //public int characterWrapLimit;

    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    //public void SetText(string content, string header = "", string cost = "")
    //{
    //    if (string.IsNullOrEmpty(header))
    //    {
    //        headerField.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        headerField.gameObject.SetActive(true);
    //        headerField.text = header;
    //    }

    //    //characterIndex = CharacterSelect.GetCharacterIndex();

    //    contentField.text = content;
    //    costField.text = cost;

    //    int headerLength = headerField.text.Length;
    //    int contentLength = contentField.text.Length;

    //    layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    //}

    private void Update()
    {
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }
}
