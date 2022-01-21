using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherBlaster : Tower
{
    [SerializeField] float featherSpeed;
    [SerializeField] GameObject featherPrefab;

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

        var feather = Instantiate(featherPrefab, firePoint.position, Quaternion.identity);

        feather.transform.LookAt(enemy.transform);
        feather.GetComponentInChildren<Rigidbody>().velocity = direction * featherSpeed;
        feather.GetComponentInChildren<Feather>().SetTower(this);
    }

    public override void Upgrade()
    {
        if (towerLevel == 1)
        {
            attackRange += 5f;
        }
        else if (towerLevel == 2)
        {
            towerDamage *= 1.5f;
        }
        else if (towerLevel == 3)
        {

        }

        base.Upgrade();
    }
}
