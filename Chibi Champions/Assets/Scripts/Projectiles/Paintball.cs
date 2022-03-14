using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintball : MonoBehaviour
{
    float bulletDamage = 5;
    float deactivateDelay = 2;

    private void Update()
    {
        bulletDamage = FindObjectOfType<Potter>().GetLightAttackDamage();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<Health>().ModifyHealth(-bulletDamage);

            ParticleManager.Instance.SpawnParticle(ParticleTypes.Hurt, collision.contacts[0].point);
        }

        ProjectilePool.Instance.AddToPaintballPool(gameObject.transform.parent.gameObject);
    }

    public void StartDelay()
    {
        StartCoroutine(DelayToDeactivate());
    }

    IEnumerator DelayToDeactivate()
    {
        yield return new WaitForSeconds(deactivateDelay);

        ProjectilePool.Instance.AddToPaintballPool(gameObject.transform.parent.gameObject);
    }
}
