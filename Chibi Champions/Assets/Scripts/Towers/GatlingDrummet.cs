using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingDrummet : Tower
{
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        UpdateView();

        if (targetEnemy == null)
        {
            return;
        }

        transform.LookAt(targetEnemy.transform.position);

        if (CanAttack())
        {
            Attack(targetEnemy);
        }
    }

    protected override void Attack(GameObject enemy = null)
    {
        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        var feather = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        feather.transform.LookAt(enemy.transform);
        feather.GetComponentInChildren<Rigidbody>().velocity = direction * bulletSpeed;
        feather.GetComponentInChildren<Feather>().SetTower(this);
    }

    public override void Upgrade()
    {
        if (towerLevel == 1)
        {
            bulletSpeed *= 1.25f;
        }
        else if (towerLevel == 2)
        {
            attackDelay /= 1.5f;
        }
        else if (towerLevel == 3)
        {
            towerDamage *= 1.5f;
        }

        base.Upgrade();
    }
}
