using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Cure : MonoBehaviour
{
    TextMeshProUGUI healthText;
    bool alertFired;
    bool alert2Fired;
    bool alert3Fired;

    bool loseStarted;

    [SerializeField] TMP_Text cureUIHPText;

    private void Start()
    {
        healthText = GetComponentInChildren<TextMeshProUGUI>();
        cureUIHPText.text = GetComponent<Health>().GetCurrentHealth().ToString() + " / 100";
    }
    // Update is called once per frame
    void Update()
    {
        healthText.text = GetComponent<Health>().GetCurrentHealth() + "/" + GetComponent<Health>().GetMaxHealth();

        cureUIHPText.text = GetComponent<Health>().GetCurrentHealth().ToString() + " / " + GetComponent<Health>().GetMaxHealth();

        if (GetComponent<Health>().GetCurrentHealth() < 0)
        {
            cureUIHPText.text = "0 / " + GetComponent<Health>().GetMaxHealth();
            healthText.text = " 0 / " + GetComponent<Health>().GetMaxHealth();
        }

        transform.RotateAround(transform.position, new Vector3(0, 1, 0), 0.1f);

        if (GetComponent<Health>().GetCurrentHealth() <= 50 && !alertFired)
        {
            AlertManager.Instance.DisplayAlert(new Alert(Color.red, "The Cure Is At 50 HP!", 1));
            alertFired = true;

            FindObjectOfType<AudioManager>().Play("Cure Damage");
        }
        if (GetComponent<Health>().GetCurrentHealth() <= 25 && !alert2Fired)
        {
            AlertManager.Instance.DisplayAlert(new Alert(Color.red, "The Cure Is At 25 HP!", 2));
            alert2Fired = true;

            FindObjectOfType<AudioManager>().Play("Cure Damage");
        }
        if (GetComponent<Health>().GetCurrentHealth() <= 10 && !alert3Fired)
        {
            AlertManager.Instance.DisplayAlert(new Alert(Color.red, "The Cure Only Has 10 HP Left!", 2));
            alert3Fired = true;

            FindObjectOfType<AudioManager>().Play("Cure Damage");
        }


        if (GetComponent<Health>().GetCurrentHealth() <= 0 && !loseStarted)
        {
            StartCoroutine(DelayBeforeLoss());

            loseStarted = true;
        }
    }

    IEnumerator DelayBeforeLoss()
    {
        if (FindObjectOfType<UDPClient>() != null)
        {
            UDPClient.Instance.SendLeaderboardStats();
        }

        AlertManager.Instance.DisplayAlert(new Alert(Color.blue, "You Lose!", 2));       

        yield return new WaitForSeconds(3);

        CanvasManager.Instance.RemoveCursorLock();

        SceneManager.LoadScene("Lose");
    }
}
