using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthPack : MonoBehaviour
{
    [SerializeField] float cooldown;
    [SerializeField] GameObject healthpackMesh;

    [SerializeField] TMP_Text cooldownText;
    float currentTime = 0;

    bool startCooldown = false;

    private void Start()
    {
        currentTime = cooldown;
        cooldownText.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), 20 * Time.deltaTime);

        if (startCooldown)
        {
            currentTime -= 1 * Time.deltaTime;
            cooldownText.text = currentTime.ToString("0");       
        }

        if (currentTime <= 0)
        {
            startCooldown = false;
            cooldownText.text = string.Empty;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject.GetComponentInParent<Health>().GetCurrentHealth() < 100)
            {
                other.gameObject.GetComponentInParent<Health>().ModifyHealth(25);

                healthpackMesh.SetActive(false);
                GetComponent<BoxCollider>().enabled = false;
                startCooldown = true;
                currentTime = cooldown;

                StartCoroutine(HealthpackCooldown());
            }
        }
    }

    IEnumerator HealthpackCooldown()
    {
        yield return new WaitForSeconds(cooldown);

        healthpackMesh.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
    }
}
