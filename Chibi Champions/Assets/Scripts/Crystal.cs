using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Crystal : MonoBehaviour
{

    TextMeshProUGUI healthText;

    private void Start()
    {
        healthText = GetComponentInChildren<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        healthText.text = GetComponent<Health>().GetCurrentHealth() + "/" + GetComponent<Health>().GetMaxHealth();

        transform.RotateAround(transform.position, new Vector3(0, 1, 0), 0.1f);
    }
}
