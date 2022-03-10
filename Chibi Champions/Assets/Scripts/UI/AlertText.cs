using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertText : MonoBehaviour
{
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

    public void SetInfo(Alert info)
    {
        alertInfo = info;

        alert.fontSize = alertInfo.startingSize;

        if (alertInfo.delayBeforeFade > 0)
        {
            StartCoroutine(BeginDelay());
        }
        else
        {
            delayReached = true;
        }
    }

    IEnumerator BeginDelay()
    {
        yield return new WaitForSeconds(alertInfo.delayBeforeFade);

        delayReached = true;
    }
}
