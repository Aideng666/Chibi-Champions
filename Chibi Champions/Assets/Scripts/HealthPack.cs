using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] float cooldown;
    [SerializeField] GameObject healthpackMesh;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), 5 * Time.deltaTime);
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
