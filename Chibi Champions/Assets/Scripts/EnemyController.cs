using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    Transform crystalTransform;
    Transform playerTransform;
    NavMeshAgent navMeshAgent;
    EnemyAttackStates currentAttackState;

    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] float playerSpottedRange = 8;
    [SerializeField] LayerMask crystalLayer;
    [SerializeField] LayerMask playerLayer;

    float attackDelay = 1.5f;
    float timeUntilNextAttack = 0;

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
            AttackCrystal();
        }
        else
        {
            navMeshAgent.destination = playerTransform.position;
            AttackPlayer();
        }
    }

    void AttackCrystal()
    {
        if ((Vector3.Distance(transform.position, crystalTransform.position) < attackRange * 2) && CanAttack())
        {
            AnimController.Instance.PlayEnemyAttackAnim();

            Collider[] crystalHits = Physics.OverlapSphere(attackPoint.position, attackRange, crystalLayer);

            foreach (Collider crystal in crystalHits)
            {
                Debug.Log("Crystal Damaged");
            }
        }
    }

    void AttackPlayer()
    {
        if ((Vector3.Distance(transform.position, playerTransform.position) < attackRange * 2) && CanAttack())
        {
            AnimController.Instance.PlayEnemyAttackAnim();

            Collider[] playerHits = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

            foreach (Collider player in playerHits)
            {
                Debug.Log("Player Damaged");
            }
        }
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

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.red;
    }
}
