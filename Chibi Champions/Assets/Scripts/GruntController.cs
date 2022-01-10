using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntController : MonoBehaviour
{
    [SerializeField] LayerMask crystalLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] float playerSpottedRange = 8;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Health>().GetCurrentHealth() <= 0)
        {
            EnemyPool.Instance.AddToGruntPool(gameObject);
        }

        if (Vector3.Distance(transform.position, playerTransform.position) < playerSpottedRange)
        {
            currentAttackState = EnemyAttackStates.Player;
        }
        else
        {
            currentAttackState = EnemyAttackStates.Crystal;
        }


        if (currentAttackState == EnemyAttackStates.Crystal && navMeshAgent.gameObject.activeSelf)
        {
            navMeshAgent.destination = crystalTransform.position;

            if ((Vector3.Distance(transform.position, crystalTransform.position) < attackRange * 2) && CanAttack())
            {
                AnimController.Instance.PlayEnemyAttackAnim(GetComponent<Animator>());

                StartCoroutine(DelayBeforeAttack());
            }

            if (delayBeforeAttackReached)
            {
                AttackCrystal();
            }
        }
        else if (currentAttackState == EnemyAttackStates.Player && navMeshAgent.gameObject.activeSelf)
        {
            navMeshAgent.destination = playerTransform.position;

            if ((Vector3.Distance(transform.position, playerTransform.position) < attackRange * 2) && CanAttack())
            {
                AnimController.Instance.PlayEnemyAttackAnim(GetComponent<Animator>());

                StartCoroutine(DelayBeforeAttack());
            }

            if (delayBeforeAttackReached)
            {
                AttackPlayer();
            }
        }
    }

    void AttackCrystal()
    {
        Collider[] crystalHits = Physics.OverlapSphere(attackPoint.position, attackRange, crystalLayer);

        foreach (Collider crystal in crystalHits)
        {
            crystal.gameObject.GetComponent<Health>().ModifyHealth(-1);
        }

        delayBeforeAttackReached = false;
    }

    void AttackPlayer()
    {
        Collider[] playerHits = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        foreach (Collider player in playerHits)
        {
            player.gameObject.GetComponentInParent<Health>().ModifyHealth(-5);
        }

        delayBeforeAttackReached = false;
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

    IEnumerator DelayBeforeAttack()
    {
        yield return new WaitForSeconds(0.2f);

        delayBeforeAttackReached = true;
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
