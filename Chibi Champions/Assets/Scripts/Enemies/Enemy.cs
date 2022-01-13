using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float attackRange;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected float playerSpottedRange;

    protected Transform crystalTransform;
    protected Transform playerTransform;
    protected NavMeshAgent navMeshAgent;
    protected EnemyAttackStates currentAttackState;

    protected float attackDelay = 1.5f;
    protected float timeUntilNextAttack = 0;

    protected bool delayBeforeAttackReached = false;

    protected bool knockbackApplied;

    // Start is called before the first frame update
    protected void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentAttackState = EnemyAttackStates.Crystal;

        crystalTransform = FindObjectOfType<Crystal>().transform;
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < playerSpottedRange)
        {
            currentAttackState = EnemyAttackStates.Player;
        }
        else
        {
            currentAttackState = EnemyAttackStates.Crystal;
        }

        if (currentAttackState == EnemyAttackStates.Crystal && navMeshAgent.gameObject.activeSelf && !knockbackApplied)
        {
            navMeshAgent.destination = crystalTransform.position;

            if ((Vector3.Distance(transform.position, crystalTransform.position) < attackRange * 2) && CanAttack())
            {
                StartCoroutine(DelayBeforeAttack());
            }

            if (delayBeforeAttackReached)
            {
                AttackCrystal();
            }
        }
        else if (currentAttackState == EnemyAttackStates.Player && navMeshAgent.gameObject.activeSelf && !knockbackApplied)
        {
            navMeshAgent.destination = playerTransform.position;

            if ((Vector3.Distance(transform.position, playerTransform.position) < attackRange * 2) && CanAttack())
            {
                StartCoroutine(DelayBeforeAttack());
            }

            if (delayBeforeAttackReached)
            {
                AttackPlayer();
            }
        }
    }
    protected bool CanAttack()
    {
        if (timeUntilNextAttack < Time.realtimeSinceStartup)
        {
            timeUntilNextAttack = Time.realtimeSinceStartup + attackDelay;
            return true;
        }

        return false;
    }
    public IEnumerator Knockback()
    {
        knockbackApplied = true;

        Vector3 tempDestination = (transform.position - playerTransform.position) * 3;
        Vector3 originalDestination = navMeshAgent.destination;

        navMeshAgent.destination = tempDestination;
        navMeshAgent.speed *= 2;

        yield return new WaitForSeconds(0.5f);

        navMeshAgent.destination = originalDestination;
        navMeshAgent.speed /= 2;

        knockbackApplied = false;
    }

    protected virtual IEnumerator DelayBeforeAttack()
    {
        yield return null;
    }

    protected virtual void AttackCrystal()
    {

    }

    protected virtual void AttackPlayer()
    {

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
