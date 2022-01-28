using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    Tower tower;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<Health>().ModifyHealth(-tower.GetDamage());
            Destroy(gameObject);
        }

    }

    public void SetTower(Tower t)
    {
        tower = t;
    }
}
