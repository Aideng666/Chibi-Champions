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

    [SerializeField] AudioSource hit;

    protected Transform cureTransform;
    protected Transform playerTransform;
    protected PlayerController[] playerControllers;
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

    protected int level = 1;

    // Start is called before the first frame update
    protected void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentAttackState = EnemyAttackStates.Cure;

        cureTransform = FindObjectOfType<Cure>().transform;
        playerControllers = FindObjectsOfType<PlayerController>();

        navMeshAgent.speed = defaultSpeed;
        hit.volume = FindObjectOfType<AudioManager>().GetSFXVolume();

    }

    // Update is called once per frame
    protected void Update()
    {
        if (FindObjectOfType<AudioManager>().dirtyNME)
        {
            if (FindObjectOfType<AudioManager>().isMute() == true)
            {
                hit.mute = true;
            }
            else
            {
                hit.mute = false;
            }

            hit.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
            FindObjectOfType<AudioManager>().dirtyNME = false;

        }
        TennisBall[] tennisBalls = ObjectManager.Instance.GetTennisBalls();

        if (!knockbackApplied)
        {
            playerTransform = playerControllers[0].transform;

            foreach(PlayerController player in playerControllers)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < Vector3.Distance(playerTransform.position, transform.position))
                {
                    playerTransform = player.transform;
                }
            }

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
                else
                {
                    if (Vector3.Distance(transform.position, playerTransform.position) < playerSpottedRange && playerTransform.GetComponent<PlayerController>().GetIsAlive())
                    {
                        currentAttackState = EnemyAttackStates.Player;
                    }
                    else
                    {
                        currentAttackState = EnemyAttackStates.Cure;
                    }
                }
            }
            else if (Vector3.Distance(transform.position, playerTransform.position) < playerSpottedRange && playerTransform.GetComponent<PlayerController>().GetIsAlive())
            {
                currentAttackState = EnemyAttackStates.Player;
            }
            else
            {
                currentAttackState = EnemyAttackStates.Cure;
            }

            if (currentAttackState == EnemyAttackStates.Cure && navMeshAgent.gameObject.activeSelf)
            {
                navMeshAgent.destination = cureTransform.position;

                if ((Vector3.Distance(transform.position, cureTransform.position) < attackRange * 2.5f) && CanAttack())
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

                if ((Vector3.Distance(transform.position, playerTransform.position) < attackRange * 2.2f) && CanAttack())
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
        }

        if (!isGrounded && GetComponent<Rigidbody>().velocity.y < 0.01 && GetComponent<Rigidbody>().velocity.y > -0.01)
        {
            Destroy(GetComponent<Rigidbody>());

            navMeshAgent.enabled = true;

            knockbackApplied = false;

            isGrounded = true;

            ParticleManager.Instance.SpawnParticle(ParticleTypes.JumpLanding, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z));
        }
        ///////////////////////////////////////////////////////////////////////////////////////
    }
    protected bool CanAttack()
    {
        if (timeUntilNextAttack < Time.time)
        {
            timeUntilNextAttack = Time.time + attackDelay;
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
            if (timeToNextEffectTick < Time.time)
            {
                timeToNextEffectTick = Time.time + effectTickDelay;

                GetComponent<Health>().ModifyHealth(-2);

                Knockback(5, FindObjectOfType<Cure>().transform);
            }
        }
    }
    public void Knockback(float knockbackForce, Transform origin)
    {
        Rigidbody body = new Rigidbody();

        hit.Play();

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

        if (GetComponent<Rigidbody>() != null)
        {
            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

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

    public void HitSound()
    {
        hit.Play();
    }

    public virtual void SetLevel(int lvl)
    {
        level = lvl;
    }

    public int GetLevel()
    {
        return level;
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
