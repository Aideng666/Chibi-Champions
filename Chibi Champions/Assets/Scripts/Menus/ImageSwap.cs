using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwap : MonoBehaviour
{
    public Image portrait;
    public Sprite oldImage, newImage;

    public void ImageChange()
    {
        portrait.sprite = newImage;
    }

    public void ImageRevert()
    {
        portrait.sprite = oldImage;
    }
}
