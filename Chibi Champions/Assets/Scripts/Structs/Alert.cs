using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Alert
{
    public float lifespan;
    public float startingSize;
    public float endSize;
    public float delayBeforeFade;
    public string text;
    public Color color;

    public Alert(Color alertColor, string alertText, float delay = 0, float span = 2, float startSize = 36, float finishSize = 0)
    {
        color = alertColor;
        text = alertText;
        delayBeforeFade = delay;
        lifespan = span;
        startingSize = startSize;
        endSize = finishSize;
    }
}
