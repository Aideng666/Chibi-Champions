using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenLaser : Tower
{
    LineRenderer laserbeam;
    bool slowEnemies;

    [SerializeField] AudioSource beam;

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<AudioManager>().isMute() == true)
        {
            beam.mute = true;
        }
        else
        {
            beam.mute = false;
        }
        beam.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
        laserbeam = GetComponentInChildren<LineRenderer>();

        UpdateView();

        if (targetEnemy == null)
        {
            if (laserbeam.enabled)
            {
                laserbeam.enabled = false;
                beam.Stop();
            }

            return;
        }

        transform.LookAt(new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y + 1f, targetEnemy.transform.position.z));

        Attack(targetEnemy);
    }

    protected override void Attack(GameObject enemy = null)
    {
        if (!laserbeam.enabled)
        {
            laserbeam.enabled = true;
            beam.Play();
        }

        laserbeam.SetPosition(0, firePoint.position);
        laserbeam.SetPosition(1, enemy.transform.position);

        enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-towerDamage);

        if (slowEnemies)
        {
            enemy.gameObject.GetComponentInParent<NavMeshAgent>().speed = 2;
        }
        else
        {
            enemy.gameObject.GetComponentInParent<NavMeshAgent>().speed = enemy.GetComponentInParent<Enemy>().GetDefaultSpeed();
        }
    }

    public override void Upgrade()
    {
        if (towerLevel == 1)
        {
            attackRange += 5f;
        }
        else if(towerLevel == 2)
        {
            towerDamage *= 1.5f;
        }
        else if (towerLevel == 3)
        {
            slowEnemies = true;
        }
        else
        {
            print("Tower is Max Level");
            return;
        }

        base.Upgrade();
    }
}
