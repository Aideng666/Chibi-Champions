using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingNeedle : MonoBehaviour
{
    [SerializeField] float healingRadius = 3;
    float healAmount = 15;

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, healingRadius);

        foreach (Collider player in hitPlayers)
        {
            if (player.tag == "Player")
            {
                player.GetComponentInParent<Health>().ModifyHealth(FindObjectOfType<Potter>().GetHealAmount());
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, healingRadius);
    }
}
