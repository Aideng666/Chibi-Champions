using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    Tower tower;
    float bulletDamage = 5;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<Health>().ModifyHealth(-bulletDamage);
            Destroy(gameObject);
        }

    }

    public void SetTower(Tower t)
    {
        tower = t;
        bulletDamage = tower.GetDamage();
    }
}
