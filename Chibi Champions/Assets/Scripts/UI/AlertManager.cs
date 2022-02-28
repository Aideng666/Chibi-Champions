using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertManager : MonoBehaviour
{
    [SerializeField] GameObject alertPrefab;

    public static AlertManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void DisplayAlert(Alert alertInfo)
    {
        Instantiate(alertPrefab, gameObject.transform).GetComponent<AlertText>().SetInfo(alertInfo);

        //alert.GetComponent<TextMeshProUGUI>().text = alertInfo.text;
        //alert.GetComponent<AlertText>().SetLifespan(alertInfo.lifespan);
        //alert.GetComponent<AlertText>().SetStartSize(alertInfo.startingSize);
        //alert.GetComponent<AlertText>().SetEndSize(alertInfo.endSize);
        //alert.GetComponent<AlertText>().SetColor(alertInfo.color);
        //alert.GetComponent<AlertText>().SetDelay(alertInfo.delayBeforeFade);
    }
}
