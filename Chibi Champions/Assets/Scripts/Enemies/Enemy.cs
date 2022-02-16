using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected float playerSpottedRange;
    [SerializeField] protected float defaultSpeed;
    [SerializeField] protected float attackDelay = 1.5f;

    protected Transform crystalTransform;
    protected Transform playerTransform;
    protected NavMeshAgent navMeshAgent;
    protected EnemyAttackStates currentAttackState;

    protected float timeUntilNextAttack = 0;

    protected bool delayBeforeAttackReached = false;

    protected bool knockbackApplied;

    protected PlayerController lastHit;
    TennisBall closestBall;

    Effects currentEffect = Effects.None;
    float effectTickDelay;
    float timeToNextEffectTick = 0;

    bool isGrounded = true;

    // Start is called before the first frame update
    protected void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentAttackState = EnemyAttackStates.Crystal;

        crystalTransform = FindObjectOfType<Cure>().transform;
        playerTransform = FindObjectOfType<PlayerController>().transform;

        navMeshAgent.speed = defaultSpeed;
    }

    // Update is called once per frame
    protected void Update()
    {
        TennisBall[] tennisBalls = FindObjectsOfType<TennisBall>();

        if (!knockbackApplied)
        {
            if (tennisBalls.Length > 0)
            {
                closestBall = tennisBalls[0];

                foreach (TennisBall ball in tennisBalls)
                {
                    if (Vector3.Distance(transform.position, ball.transform.position) < Vector3.Distance(transform.position, closestBall.transform.position))
                    {
                        closestBall = ball;
                    }
                }

                if (Vector3.Distance(transform.position, closestBall.transform.position) < playerSpottedRange * 1.5f)
                {
                    currentAttackState = EnemyAttackStates.Tennis;
                }
            }
            else if (Vector3.Distance(transform.position, playerTransform.position) < playerSpottedRange)
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

                if ((Vector3.Distance(transform.position, playerTransform.position) < attackRange * 2) && CanAttack())
                {
                    StartCoroutine(DelayBeforeAttack());
                }

                if (delayBeforeAttackReached)
                {
                    AttackPlayer();
                }
            }
            else if (currentAttackState == EnemyAttackStates.Tennis && navMeshAgent.gameObject.activeSelf)
            {
                navMeshAgent.destination = closestBall.transform.position;
            }
        }

        ApplyEffect();

        ////Deactivates/Reactivates the Rigidbody and NavMeshAgent to account for Knockback////
        if (GetComponent<Rigidbody>() != null && GetComponent<Rigidbody>().velocity.y != 0 && isGrounded)
        {
            isGrounded = false;
            print("Left Ground");
        }

        if (!isGrounded && GetComponent<Rigidbody>().velocity.y < 0.01 && GetComponent<Rigidbody>().velocity.y > -0.01)
        {
            print("Touched Ground");

            Destroy(GetComponent<Rigidbody>());

            navMeshAgent.enabled = true;

            knockbackApplied = false;

            isGrounded = true;
        }
        ///////////////////////////////////////////////////////////////////////////////////////
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
    void ApplyEffect()
    {
        if (currentEffect == Effects.None)
        {
            return;
        }
        else if (currentEffect == Effects.Spider)
        {
            if (timeToNextEffectTick < Time.realtimeSinceStartup)
            {
                timeToNextEffectTick = Time.realtimeSinceStartup + effectTickDelay;

                GetComponent<Health>().ModifyHealth(-2);

                Knockback(5, FindObjectOfType<Cure>().transform);
            }
        }
    }
    public void Knockback(float knockbackForce, Transform origin)
    {
        Rigidbody body = new Rigidbody();

        if (!knockbackApplied)
        {
            knockbackApplied = true;

            body = gameObject.AddComponent<Rigidbody>();

            navMeshAgent.enabled = false;
        }
        else
        {
            body = GetComponent<Rigidbody>();
        }

        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Vector3 direction = (transform.position - origin.position).normalized;
        direction.y = 1;

        direction = direction.normalized;

        body.mass = 10;

        body.AddForce(direction * knockbackForce, ForceMode.Impulse);
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

    public void SetLastHit(PlayerController player)
    {
        lastHit = player;
    }

    public float GetDefaultSpeed()
    {
        return defaultSpeed;
    }

    public Effects GetCurrentEffect()
    {
        return currentEffect;
    }

    public void SetEffect(Effects effect)
    {
        currentEffect = effect;
    }

    public void SetEffectTickDelay(float delay)
    {
        effectTickDelay = delay;
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
