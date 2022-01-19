using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SharpshooterController : Enemy
{
    [SerializeField] LayerMask enemyLayer;

    LineRenderer bulletTrail;

    Vector3 shotDirection;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        navMeshAgent.stoppingDistance = 10;

        bulletTrail = GetComponentInChildren<LineRenderer>();

        bulletTrail.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (gameObject.GetComponent<Health>().GetCurrentHealth() <= 0)
        {
            lastHit.GetComponent<PointsManager>().AddPoints(50);
            EnemyPool.Instance.AddToShooterPool(gameObject);
        }
    }

    protected override void AttackCrystal()
    {
        Ray ray = new Ray(attackPoint.position, shotDirection);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, ~enemyLayer))
        {
            var selection = hit.transform;

            bulletTrail.enabled = true;

            bulletTrail.SetPosition(0, attackPoint.position);
            bulletTrail.SetPosition(1, selection.position);

            if (selection.tag == "Player")
            {
                print("Player Shot");

                playerTransform.gameObject.GetComponentInParent<Health>().ModifyHealth(-10);

            }
        }

        StartCoroutine(TurnOffBulletTrail());

        delayBeforeAttackReached = false;
        navMeshAgent.isStopped = false;
        AnimController.Instance.SetEnemyIsWalking(GetComponent<Animator>(), true);
    }

    protected override void AttackPlayer()
    {
        Ray ray = new Ray(attackPoint.position, shotDirection);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, ~enemyLayer))
        {
            var selection = hit.transform;

            bulletTrail.enabled = true;

            bulletTrail.SetPosition(0, attackPoint.position);
            bulletTrail.SetPosition(1, selection.position);

            if (selection.tag == "Player")
            {
                print("Player Shot");

                playerTransform.gameObject.GetComponentInParent<Health>().ModifyHealth(-10);

            }
        }

        StartCoroutine(TurnOffBulletTrail());

        delayBeforeAttackReached = false;
        navMeshAgent.isStopped = false;
        AnimController.Instance.SetEnemyIsWalking(GetComponent<Animator>(), true);
    }

    protected override IEnumerator DelayBeforeAttack()
    {
        if ((Vector3.Distance(transform.position, playerTransform.position) < attackRange))
        {
            navMeshAgent.isStopped = true;

            AnimController.Instance.SetEnemyIsWalking(GetComponent<Animator>(), false);
        }

        yield return new WaitForSeconds(0.8f);


        if (currentAttackState == EnemyAttackStates.Player)
        {
            shotDirection = (playerTransform.position - attackPoint.position).normalized;
        }
        else if (currentAttackState == EnemyAttackStates.Crystal)
        {
            shotDirection = (crystalTransform.position - attackPoint.position).normalized;
        }

        yield return new WaitForSeconds(0.2f);

        delayBeforeAttackReached = true;
    }

    IEnumerator TurnOffBulletTrail()
    {
        yield return new WaitForSeconds(1f);

        bulletTrail.enabled = false;
    }
}
