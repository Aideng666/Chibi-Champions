using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingNeedle : MonoBehaviour
{
    float healAmount = 15;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<Health>().ModifyHealth(FindObjectOfType<Potter>().GetHealAmount());
        }

        Destroy(gameObject);
    }
}
