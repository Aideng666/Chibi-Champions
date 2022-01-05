using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertText : MonoBehaviour
{
    float alertLength = 2;
    float alertLifespan = 0;

    float startingSize = 36;
    float endSize = 0;

    TextMeshProUGUI alert;

    private void OnEnable()
    {
        alert = GetComponent<TextMeshProUGUI>();

        alert.fontSize = startingSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (alertLifespan >= alertLength)
        {
            gameObject.SetActive(false);
        }

        float t = alertLifespan / alertLength;

        alert.fontSize = Mathf.Lerp(startingSize, endSize, t);

        alertLifespan += Time.deltaTime;
    }

    public void SetAlert(string alertText)
    {
        alert.text = alertText;
    }
}
