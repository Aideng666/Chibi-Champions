using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenLaser : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] TowerAttackPriority defaultAttackPriority;
    [SerializeField] Transform firePoint;
    [SerializeField] float attackRange;
    [SerializeField] float laserDamage;

    TowerAttackPriority currentAttackPriority;
    LineRenderer laserbeam;

    // Start is called before the first frame update
    void Start()
    {
        currentAttackPriority = defaultAttackPriority;
        laserbeam = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (hitEnemies == null || hitEnemies.Length < 1)
        {
            if (laserbeam.enabled)
            {
                laserbeam.enabled = false;
            }

            return;
        }

        Collider closestEnemy = hitEnemies[0];

        Collider currentEnemyCheck = hitEnemies[0];

        if (currentAttackPriority == TowerAttackPriority.ClosestToTower)
        {
            for (int i = 0; i < hitEnemies.Length; i++)
            {
                currentEnemyCheck = hitEnemies[i];

                if (Vector3.Distance(currentEnemyCheck.transform.position, transform.position) < Vector3.Distance(closestEnemy.transform.position, transform.position))
                {
                    closestEnemy = currentEnemyCheck;
                }
            }

            Attack(closestEnemy.gameObject);
        }
        
    }

    void Attack(GameObject enemy)
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
