using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SharpshooterController : MonoBehaviour
{
    [SerializeField] float attackRange;
    [SerializeField] float playerSpottedRange = 15;

    Transform crystalTransform;
    Transform playerTransform;
    NavMeshAgent navMeshAgent;
    EnemyAttackStates currentAttackState;


    float attackDelay = 1.5f;
    float timeUntilNextAttack = 0;

    bool delayBeforeAttackReached = false;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentAttackState = EnemyAttackStates.Crystal;

        crystalTransform = FindObjectOfType<Crystal>().transform;
        playerTransform = FindObjectOfType<PlayerController>().transform;

        navMeshAgent.stoppingDistance = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Health>().GetCurrentHealth() <= 0)
        {
            EnemyPool.Instance.AddToShooterPool(gameObject);
        }

        if (Vector3.Distance(transform.position, playerTransform.position) < playerSpottedRange)
        {
            currentAttackState = EnemyAttackStates.Player;
        }
        else
        {
            currentAttackState = EnemyAttackStates.Crystal;
        }

        if (currentAttackState == EnemyAttackStates.Crystal)
        {
            navMeshAgent.destination = crystalTransform.position;

            if ((Vector3.Distance(transform.position, crystalTransform.position) < attackRange * 2) && CanAttack())
            {
                //AnimController.Instance.PlayEnemyAttackAnim(GetComponent<Animator>());

                StartCoroutine(DelayBeforeAttack());
                //AttackCrystal();
            }

            if (delayBeforeAttackReached)
            {
                AttackCrystal();
            }
        }
        else
        {
            navMeshAgent.destination = playerTransform.position;

            if ((Vector3.Distance(transform.position, playerTransform.position) < attackRange * 2) && CanAttack())
            {
                //AnimController.Instance.PlayEnemyAttackAnim(GetComponent<Animator>());

                StartCoroutine(DelayBeforeAttack());
                //AttackPlayer();
            }

            if (delayBeforeAttackReached)
            {
                AttackPlayer();
            }
        }
    }

    void AttackCrystal()
    {
        //Collider[] crystalHits = Physics.OverlapSphere(attackPoint.position, attackRange, crystalLayer);

        //foreach (Collider crystal in crystalHits)
        //{
        //    crystal.gameObject.GetComponent<Health>().ModifyHealth(-1);
        //}

        delayBeforeAttackReached = false;
        navMeshAgent.isStopped = false;
    }

    void AttackPlayer()
    {
        //Collider[] playerHits = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        //foreach (Collider player in playerHits)
        //{
        //    player.gameObject.GetComponentInParent<Health>().ModifyHealth(-5);
        //}
        print("Shot Player");


        delayBeforeAttackReached = false;
        navMeshAgent.isStopped = false;
    }

    IEnumerator DelayBeforeAttack()
    {
        navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(0.5f);

        delayBeforeAttackReached = true;
    }

    bool CanAttack()
    {
        if (timeUntilNextAttack < Time.realtimeSinceStartup)
        {
            timeUntilNextAttack = Time.realtimeSinceStartup + attackDelay;
            return true;
        }

        return false;
    }
}
