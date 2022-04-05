using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkBlob : MonoBehaviour
{

    Tower tower;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            tower.GetComponent<InkBomber>().ApplyStunEffect(collision.gameObject);

            collision.gameObject.GetComponentInParent<Health>().ModifyHealth(-tower.GetDamage());

            ParticleManager.Instance.SpawnParticle(ParticleTypes.Ink, transform.position);
            Destroy(gameObject);
        }

    }



    public void SetTower(Tower t)
    {
        tower = t;
    }
}
