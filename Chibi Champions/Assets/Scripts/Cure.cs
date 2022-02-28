using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cure : MonoBehaviour
{
    TextMeshProUGUI healthText;
    bool alertFired;
    bool alert2Fired;
    bool alert3Fired;

    private void Start()
    {
        healthText = GetComponentInChildren<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        healthText.text = GetComponent<Health>().GetCurrentHealth() + "/" + GetComponent<Health>().GetMaxHealth();

        transform.RotateAround(transform.position, new Vector3(0, 1, 0), 0.1f);

        if (GetComponent<Health>().GetCurrentHealth() <= 50 && !alertFired)
        {
            AlertManager.Instance.DisplayAlert(new Alert(Color.red, "The Cure Is At 50 HP!"));
            alertFired = true;
        }
        if (GetComponent<Health>().GetCurrentHealth() <= 25 && !alert2Fired)
        {
            AlertManager.Instance.DisplayAlert(new Alert(Color.red, "The Cure Is At 25 HP!"));
            alert2Fired = true;
        }
        if (GetComponent<Health>().GetCurrentHealth() <= 10 && !alert3Fired)
        {
            AlertManager.Instance.DisplayAlert(new Alert(Color.red, "The Cure Only Has 10 HP Left!"));
            alert3Fired = true;
        }

    }
}
