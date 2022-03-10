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

        print("Info Sent: " + alertInfo.delayBeforeFade);
    }
}
