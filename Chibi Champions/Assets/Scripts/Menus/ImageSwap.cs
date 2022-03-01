using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwap : MonoBehaviour
{
    public Image oldImage;
    public Sprite newImage;
    public Sprite oldSprite;

    public void ImageChange()
    {
        oldImage.sprite = newImage;
    }

    public void RevertImage()
    {
        oldImage.sprite = oldSprite;
    }
}
