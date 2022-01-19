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
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (gameObject.GetComponent<Health>().GetCurrentHealth() <= 0)
        {
            lastHit.GetComponent<PointsManager>().AddPoints(50);
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
            player.gameObject.GetComponentInParent<Health>().ModifyHealth(-5);
        }

        delayBeforeAttackReached = false;
    }

    protected override IEnumerator DelayBeforeAttack()
    {
        AnimController.Instance.PlayEnemyAttackAnim(GetComponent<Animator>());

        yield return new WaitForSeconds(0.2f);

        delayBeforeAttackReached = true;
    }
}
