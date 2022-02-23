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

            Destroy(gameObject);
        }

    }

    public void SetTower(Tower t)
    {
        tower = t;
    }
}
