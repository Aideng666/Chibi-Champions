using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] protected string[] upgradeNames = new string[3];
    [SerializeField] protected Sprite[] upgradeImages = new Sprite[3];
    [SerializeField] protected string towerName;

    protected TowerAttackPriority currentAttackPriority;
    
    protected GameObject targetEnemy = null;

    protected int towerLevel = 1;

    protected float timeToNextAttack = 0;

    protected int totalPointsSpent;

    private void Start()
    {
        currentAttackPriority = defaultAttackPriority;

        totalPointsSpent = towerCost;
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

                if (Vector3.Distance(currentEnemyCheck.transform.position, FindObjectOfType<Cure>().transform.position) < Vector3.Distance(selectedEnemy.transform.position, FindObjectOfType<Cure>().transform.position))
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

        totalPointsSpent += upgradeCosts[towerLevel - 2];
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

    public int GetTotalPointsSpent()
    {
        return totalPointsSpent;
    }

    public int GetUpgradeCost(int level)
    {
        return upgradeCosts[level - 1];
    }

    public float GetDamage()
    {
        return towerDamage;
    }

    public float GetRange()
    {
        return attackRange;
    }

    public LayerMask GetEnemyLayer()
    {
        return enemyLayer;
    }

    public string GetUpgradeName(int level)
    {
        return upgradeNames[level - 1];
    }

    public Sprite GetUpgradeImage(int level)
    {
        return upgradeImages[level - 1];
    }

    public string GetTowerName()
    {
        return towerName;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
