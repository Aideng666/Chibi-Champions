using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photosynthesizer : Tower
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float healingAmount;
    [SerializeField] ParticleSystem healingParticles;

    // Update is called once per frame
    void Update()
    {
        UpdateView();

        Heal();

        ParticleSystem.ShapeModule radius = healingParticles.shape;
        radius.radius = (attackRange / 2) + 1;
    }

    void Heal()
    {
        Collider[] playerHits = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

        foreach (Collider player in playerHits)
        {
            player.gameObject.GetComponentInParent<Health>().ModifyHealth(healingAmount);
        }
    }

    public override void Upgrade()
    {
        if (towerLevel == 1)
        {
            attackRange += 5f;
        }
        else if (towerLevel == 2)
        {
            healingAmount *= 2f;
        }
        else if (towerLevel == 3)
        {
            foreach(PlayerController player in FindObjectsOfType<PlayerController>())
            {
                player.GetComponent<Health>().SetMaxHealth(player.GetComponent<Health>().GetMaxHealth() + 10);
            }
        }
        else
        {
            print("Tower is Max Level");
            return;
        }

        base.Upgrade();
    }
}
