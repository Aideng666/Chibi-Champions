using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class GruntController : Enemy
{
    [SerializeField] LayerMask crystalLayer;
    [SerializeField] LayerMask playerLayer;

    [SerializeField] Image enemyLevelBG;
    [SerializeField] TMP_Text enemyLevel;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        navMeshAgent.stoppingDistance = 1;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (gameObject.GetComponent<Health>().GetCurrentHealth() <= 0)
        {
            foreach (PlayerController player in FindObjectsOfType<PlayerController>())
            {
                if (level == 1)
                {
                    player.GetComponent<PointsManager>().AddPoints(100);
                }
                else if (level == 2)
                {
                    player.GetComponent<PointsManager>().AddPoints(150);
                }
                else if (level == 3)
                {
                    player.GetComponent<PointsManager>().AddPoints(400);
                }
            }

            WaveManager.Instance.AddEnemyKilled();
            EnemyPool.Instance.AddToGruntPool(gameObject);
        }
    }

    protected override void AttackCrystal()
    {
        Collider[] crystalHits = Physics.OverlapSphere(attackPoint.position, attackRange, crystalLayer);

        foreach (Collider crystal in crystalHits)
        {
            crystal.gameObject.GetComponent<Health>().ModifyHealth(-1);
        }

        delayBeforeAttackReached = false;
    }

    protected override void AttackPlayer()
    {
        Collider[] playerHits = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        foreach (Collider player in playerHits)
        {
            player.gameObject.GetComponentInParent<Health>().ModifyHealth(-attackDamage);
        }

        delayBeforeAttackReached = false;
    }

    protected override IEnumerator DelayBeforeAttack()
    {
        AnimController.Instance.PlayEnemyAttackAnim(GetComponent<Animator>());

        yield return new WaitForSeconds(0.2f);

        delayBeforeAttackReached = true;
    }

    public override void SetLevel(int lvl)
    {
        base.SetLevel(lvl);

        if (level == 1)
        {
            GetComponent<Health>().SetMaxHealth(50);
            GetComponent<Health>().ResetHealth();
            attackDamage = 2;

            enemyLevelBG.color = Color.yellow;
            enemyLevel.text = level.ToString();
        }
        else if (level == 2)
        {
            GetComponent<Health>().SetMaxHealth(100);
            GetComponent<Health>().ResetHealth();
            attackDamage = 6;

            enemyLevelBG.color = Color.magenta;
            enemyLevel.text = level.ToString();
        }
        else if (level == 3)
        {
            GetComponent<Health>().SetMaxHealth(200);
            GetComponent<Health>().ResetHealth();
            attackDamage = 14;

            enemyLevelBG.color = Color.red;
            enemyLevel.text = level.ToString();
        }
    }
}
