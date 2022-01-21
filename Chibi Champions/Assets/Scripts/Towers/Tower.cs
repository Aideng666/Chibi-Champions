using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] protected TowerType type;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected TowerAttackPriority defaultAttackPriority;
    [SerializeField] protected float attackRange;
    [SerializeField] protected int towerCost;
    [SerializeField] protected float towerDamage;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected int[] upgradeCosts = new int[3];
    [SerializeField] protected Transform firePoint;

    protected TowerAttackPriority currentAttackPriority;
    
    protected GameObject targetEnemy = null;

    protected int towerLevel = 1;

    protected float timeToNextAttack = 0;

    private void Start()
    {
        currentAttackPriority = defaultAttackPriority;
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

    public virtual void Upgrade()
    {
        towerLevel++;
    }

    protected bool CanAttack()
    {
        if (timeToNextAttack < Time.realtimeSinceStartup)
        {
            timeToNextAttack = Time.realtimeSinceStartup + attackDelay;
            return true;
        }

        return false;
    }

    public int GetLevel()
    {
        return towerLevel;
    }

    public int GetCost()
    {
        return towerCost;
    }

    public int GetUpgradeCost(int level)
    {
        return upgradeCosts[level - 1];
    }

    public float GetDamage()
    {
        return towerDamage;
    }
}
