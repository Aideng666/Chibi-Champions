using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntController : Enemy
{
    [SerializeField] LayerMask crystalLayer;
    [SerializeField] LayerMask playerLayer;

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
            if (lastHit != null)
            {
                lastHit.GetComponent<PointsManager>().AddPoints(50);
            }

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
        }
        else if (level == 2)
        {
            GetComponent<Health>().SetMaxHealth(100);
            GetComponent<Health>().ResetHealth();
            attackDamage = 6;
        }
        else if (level == 3)
        {
            GetComponent<Health>().SetMaxHealth(200);
            GetComponent<Health>().ResetHealth();
            attackDamage = 14;
        }
    }
}
