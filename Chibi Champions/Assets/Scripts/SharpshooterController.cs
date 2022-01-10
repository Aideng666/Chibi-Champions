using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SharpshooterController : MonoBehaviour
{
    [SerializeField] float attackRange;
    [SerializeField] float playerSpottedRange = 15;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;

    Transform crystalTransform;
    Transform playerTransform;
    NavMeshAgent navMeshAgent;
    EnemyAttackStates currentAttackState;

    LineRenderer bulletTrail;

    Vector3 shotDirection;

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

        navMeshAgent.stoppingDistance = 10;

        bulletTrail = GetComponentInChildren<LineRenderer>();

        bulletTrail.enabled = false;
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

        if (currentAttackState == EnemyAttackStates.Crystal && navMeshAgent.gameObject.activeSelf)
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
        else if (currentAttackState == EnemyAttackStates.Player && navMeshAgent.gameObject.activeSelf)
        {
            navMeshAgent.destination = playerTransform.position;

            if ((Vector3.Distance(transform.position, playerTransform.position) <= attackRange) && CanAttack())
            {

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
        //Collider[] crystalHits = Physics.OverlapSphere(attackPoint.position, attackRange, crystalLayer);

        //foreach (Collider crystal in crystalHits)
        //{
        //    crystal.gameObject.GetComponent<Health>().ModifyHealth(-1);
        //}

        delayBeforeAttackReached = false;
        navMeshAgent.isStopped = false;
        AnimController.Instance.SetEnemyIsWalking(GetComponent<Animator>(), true);
    }

    void AttackPlayer()
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

    IEnumerator DelayBeforeAttack()
    {
        
        navMeshAgent.isStopped = true;

        AnimController.Instance.SetEnemyIsWalking(GetComponent<Animator>(), false);

        yield return new WaitForSeconds(0.8f);

        shotDirection = (playerTransform.position - attackPoint.position).normalized;

        shotDirection.y = 0;

        yield return new WaitForSeconds(0.2f);

        delayBeforeAttackReached = true;
    }

    IEnumerator TurnOffBulletTrail()
    {
        yield return new WaitForSeconds(1f);

        bulletTrail.enabled = false;
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
