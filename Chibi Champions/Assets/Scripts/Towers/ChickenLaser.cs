using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenLaser : Tower
{
    [SerializeField] Transform firePoint;
    [SerializeField] float laserDamage;

    LineRenderer laserbeam;

    // Start is called before the first frame update
    void Start()
    {
        laserbeam = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateView();

        if (targetEnemy == null)
        {
            if (laserbeam.enabled)
            {
                laserbeam.enabled = false;
            }

            return;
        }

        Attack(targetEnemy);
    }

    protected override void Attack(GameObject enemy = null)
    {
        if (!laserbeam.enabled)
        {
            laserbeam.enabled = true;
        }

        transform.LookAt(enemy.transform.position);

        laserbeam.SetPosition(0, firePoint.position);
        laserbeam.SetPosition(1, enemy.transform.position);

        enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-laserDamage);
    }
}
