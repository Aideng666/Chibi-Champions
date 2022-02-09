using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingDrummet : Tower
{
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject partToRotate;

    // Update is called once per frame
    void Update()
    {
        UpdateView();

        if (targetEnemy == null)
        {
            AnimController.Instance.SetGatlingDrummetFiring(GetComponentInChildren<Animator>(), false);

            return;
        }

        partToRotate.transform.LookAt(new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y - 1.5f, targetEnemy.transform.position.z));

        if (CanAttack())
        {
            Attack(targetEnemy);
        }
    }

    protected override void Attack(GameObject enemy = null)
    {
        AnimController.Instance.SetGatlingDrummetFiring(GetComponentInChildren<Animator>(), true);

        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        var feather = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        feather.transform.LookAt(enemy.transform);
        feather.GetComponentInChildren<Rigidbody>().velocity = direction * bulletSpeed;
        feather.GetComponentInChildren<Feather>().SetTower(this);

        Destroy(feather, 3);
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
        else
        {
            print("Tower is Max Level");
            return;
        }

        base.Upgrade();
    }
}
