using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertText : MonoBehaviour
{
    //float alertLifespan = 2;

    //float startingSize = 36;
    //float endSize = 0;

    //Color color;

    //float delayBeforeFade;

    float currentAlertLifetime = 0;
    bool delayReached;

    Alert alertInfo = new Alert(Color.red, "");

    TextMeshProUGUI alert;

    private void OnEnable()
    {
        alert = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        alert.text = alertInfo.text;

        if (delayReached)
        {
            if (currentAlertLifetime >= alertInfo.lifespan)
            {
                gameObject.SetActive(false);
            }

            float t = currentAlertLifetime / alertInfo.lifespan;

            alert.fontSize = Mathf.Lerp(alertInfo.startingSize, alertInfo.endSize, t);

            alert.color = alertInfo.color;

            currentAlertLifetime += Time.deltaTime;
        }
    }

    //public void SetAlert(string alertText)
    //{
    //    alert.text = alertText;
    //}

    //public void SetLifespan(float lifespan)
    //{
    //    alertLifespan = lifespan;
    //}

    //public void SetStartSize(float size)
    //{
    //    startingSize = size;
    //}

    //public void SetEndSize(float size)
    //{
    //    endSize = size;
    //}

    //public void SetColor(Color c)
    //{
    //    color = c;
    //}

    //public void SetDelay(float delay)
    //{
    //    delayBeforeFade = delay;
    //}

    public void SetInfo(Alert info)
    {
        alertInfo = info;

        alert.fontSize = alertInfo.startingSize;

        if (alertInfo.delayBeforeFade > 0)
        {
            StartCoroutine(BeginDelay());
        }
    }

    IEnumerator BeginDelay()
    {
        yield return new WaitForSeconds(alertInfo.delayBeforeFade);

        delayReached = true;
    }
}
