using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenLaser : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] TowerAttackPriority defaultAttackPriority;
    [SerializeField] float attackRange;

    TowerAttackPriority currentAttackPriority;

    // Start is called before the first frame update
    void Start()
    {
        currentAttackPriority = defaultAttackPriority;
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

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
        transform.LookAt(enemy.transform.position);
    }
}
