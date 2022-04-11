using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertManager : MonoBehaviour
{
    [SerializeField] GameObject alertPrefab;

    Queue<GameObject> alertQueue = new Queue<GameObject>();
    Queue<Alert> infoQueue = new Queue<Alert>();

    bool alertPlaying;

    //AlertText currentAlert;

    public static AlertManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!alertPlaying && alertQueue.Count > 0)
        {
            Instantiate(alertQueue.Dequeue(), transform).GetComponent<AlertText>().SetInfo(infoQueue.Dequeue());

            //currentAlert = FindObjectOfType<AlertText>();

            alertPlaying = true;
        }
    }

    public void DisplayAlert(Alert alertInfo)
    {
        infoQueue.Enqueue(alertInfo);

        alertQueue.Enqueue(alertPrefab);
    }

    public void SetAlertPlaying(bool playing)
    {
        alertPlaying = playing;
    }
}
