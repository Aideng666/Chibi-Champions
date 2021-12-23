using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Crystal : MonoBehaviour
{
    TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI alertText;
    bool alertFired;

    private void Start()
    {
        healthText = GetComponentInChildren<TextMeshProUGUI>();

        alertText.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        healthText.text = GetComponent<Health>().GetCurrentHealth() + "/" + GetComponent<Health>().GetMaxHealth();

        transform.RotateAround(transform.position, new Vector3(0, 1, 0), 0.1f);

        if (GetComponent<Health>().GetCurrentHealth() <= 50 && !alertFired)
        {
            alertText.gameObject.SetActive(true);
            alertFired = true;
        }
    }
}
