using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingNeedle : MonoBehaviour
{
    [SerializeField] float splashRadius = 6;

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, splashRadius);

        ParticleManager.Instance.SpawnParticle(ParticleTypes.InkBlast, transform.position);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                enemy.GetComponentInParent<Health>().ModifyHealth(-FindObjectOfType<Potter>().GetAbilityDamage());
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}
