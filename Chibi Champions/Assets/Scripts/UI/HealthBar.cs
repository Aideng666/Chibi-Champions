using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image healthImage;
    [SerializeField] float updateSpeedSeconds = 0.5f;

    [SerializeField] bool isPlayer = false;

    [SerializeField] TMP_Text healthText;

    private void Start()
    {
        if (isPlayer)
        {
            foreach(PlayerController player in FindObjectsOfType<PlayerController>())
            {
                if (player.GetIsPlayerCharacter())
                {
                    player.gameObject.GetComponent<Health>().OnHealthChange += HandleHealthChanged;
                }
            }
        }
        else
        {
            GetComponentInParent<Health>().OnHealthChange += HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPercent(pct));
    }

    IEnumerator ChangeToPercent(float pct)
    {
        float preChangePercent = healthImage.fillAmount;
        float elasped = 0f;

        while (elasped < updateSpeedSeconds)
        {
            elasped += Time.deltaTime;
            healthImage.fillAmount = Mathf.Lerp(preChangePercent, pct, elasped / updateSpeedSeconds);
            yield return null;
        }

        healthImage.fillAmount = pct;

        if (isPlayer)
        {
            foreach(PlayerController player in FindObjectsOfType<PlayerController>())
            {
                healthText.text = (int)player.gameObject.GetComponent<Health>().GetCurrentHealth() + " / " + player.gameObject.GetComponent<Health>().GetMaxHealth();

                if (pct < 0)
                {
                    healthText.text = "0 / " + player.gameObject.GetComponent<Health>().GetMaxHealth();
                }
            }

            //healthText.text = (pct * 100) + " / 100"; 
                      
            //if (pct < 0)
            //{
            //    healthText.text = "0 / 100";
            //}
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
