using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image healthImage;
    [SerializeField] float updateSpeedSeconds = 0.5f;

    [SerializeField] bool isPlayer = false;

    private void Start()
    {
        if (isPlayer)
        {
            FindObjectOfType<PlayerController>().gameObject.GetComponent<Health>().OnHealthChange += HandleHealthChanged;
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
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
