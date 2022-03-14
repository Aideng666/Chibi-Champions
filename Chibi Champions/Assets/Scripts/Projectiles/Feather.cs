using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    Tower tower;
    float bulletDamage = 5;
    float deactivateDelay = 2;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<Health>().ModifyHealth(-bulletDamage);

            ParticleManager.Instance.SpawnParticle(ParticleTypes.Hurt, collision.contacts[0].point);
        }

        ProjectilePool.Instance.AddToFeatherPool(gameObject.transform.parent.gameObject);
    }

    public void SetTower(Tower t)
    {
        tower = t;
        bulletDamage = tower.GetDamage();
    }

    public void StartDelay()
    {
        StartCoroutine(DelayToDeactivate());
    }

    IEnumerator DelayToDeactivate()
    {
        yield return new WaitForSeconds(deactivateDelay);

        ProjectilePool.Instance.AddToFeatherPool(gameObject.transform.parent.gameObject);
    }
}
