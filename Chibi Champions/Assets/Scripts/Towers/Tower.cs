using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] protected TowerType type;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected TowerAttackPriority defaultAttackPriority;
    [SerializeField] protected float attackRange;

    protected TowerAttackPriority currentAttackPriority;

    Tower tower;
    protected GameObject targetEnemy = null;

    private void Start()
    {
        currentAttackPriority = defaultAttackPriority;

        tower = this;

        switch (type)
        {
            case TowerType.FeatherBlaster:

                //tower = GetComponent<FeatherBlaster>();

                break;

            case TowerType.ChickenLaser:

                tower = GetComponent<ChickenLaser>();

                break;

            case TowerType.GatlingDrummet:

                break;

            case TowerType.WebShooter:

                break;

            case TowerType.TennisBomb:

                break;

            case TowerType.Watchdog:

                break;

            case TowerType.InkBomber:

                break;

            case TowerType.Photosynthesiser:

                break;

            case TowerType.PED:

                break;
        }
    }

    protected void UpdateView()
    {
        Collider[] EnemiesInView = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (EnemiesInView == null || EnemiesInView.Length < 1)
        {
            targetEnemy = null;
            return;
        }

        Collider selectedEnemy = EnemiesInView[0];

        Collider currentEnemyCheck = EnemiesInView[0];

        if (currentAttackPriority == TowerAttackPriority.ClosestToTower)
        {
            for (int i = 0; i < EnemiesInView.Length; i++)
            {
                currentEnemyCheck = EnemiesInView[i];

                if (Vector3.Distance(currentEnemyCheck.transform.position, transform.position) < Vector3.Distance(selectedEnemy.transform.position, transform.position))
                {
                    selectedEnemy = currentEnemyCheck;
                }
            }

            targetEnemy = selectedEnemy.gameObject;
        }
        else if (currentAttackPriority == TowerAttackPriority.ClosestToCrystal)
        {
            for (int i = 0; i < EnemiesInView.Length; i++)
            {
                currentEnemyCheck = EnemiesInView[i];

                if (Vector3.Distance(currentEnemyCheck.transform.position, FindObjectOfType<Crystal>().transform.position) < Vector3.Distance(selectedEnemy.transform.position, FindObjectOfType<Crystal>().transform.position))
                {
                    selectedEnemy = currentEnemyCheck;
                }
            }

            targetEnemy = selectedEnemy.gameObject;
        }
    }

    protected virtual void Attack(GameObject enemy = null)
    {

    }
}
