using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AlertText : MonoBehaviour
{
    float currentAlertLifetime = 0;
    bool delayReached;

    Alert alertInfo = new Alert(Color.red, "");

    TextMeshProUGUI alert;

    bool isActive;

    private void OnEnable()
    {
        alert = GetComponentInChildren<TextMeshProUGUI>();

        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        alert.text = alertInfo.text;

        if (delayReached)
        {
            if (currentAlertLifetime >= alertInfo.lifespan)
            {
                AlertManager.Instance.SetAlertPlaying(false);

                Destroy(gameObject);
            }

            float t = currentAlertLifetime / alertInfo.lifespan;

            alert.fontSize = Mathf.Lerp(alertInfo.startingSize, alertInfo.endSize, t);

            //Color alertColor = GetComponentInParent<Image>().color;

            //alertColor.a = Mathf.Lerp(1, 0, t);

            GetComponent<Image>().transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);

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

    public bool GetActive()
    {
        return isActive;
    }
}
