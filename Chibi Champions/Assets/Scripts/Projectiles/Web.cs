using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour
{
    Tower tower;
    bool dealDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            tower.GetComponent<WebShooter>().ApplySlowEffect(collision.gameObject);

            if (dealDamage)
            {
                collision.gameObject.GetComponentInParent<Health>().ModifyHealth(-tower.GetDamage());
            }

            Destroy(gameObject);
        }

    }

    public void SetTower(Tower t)
    {
        tower = t;
    }

    public void SetDealDamage(bool damage)
    {
        dealDamage = damage;
    }
}
