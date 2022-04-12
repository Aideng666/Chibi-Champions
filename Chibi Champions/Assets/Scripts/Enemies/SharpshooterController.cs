using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class SharpshooterController : Enemy
{
    [SerializeField] LayerMask enemyLayer;

    LineRenderer bulletTrail;

    Vector3 shotDirection;

    [SerializeField] Image enemyLevelBG;
    [SerializeField] TMP_Text enemyLevel;

    [SerializeField] AudioSource shot;

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

        //if (FindObjectOfType<AudioManager>().isMute() == true)
        //{
        //    shot.mute = true;
        //}
        //else
        //{
        //    shot.mute = false;
        //}

        //shot.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
        //shot.maxDistance = attackRange;
        //shot.minDistance = shot.maxDistance - 2;

        if (gameObject.GetComponent<Health>().GetCurrentHealth() <= 0)
        {
            foreach (PlayerController player in FindObjectsOfType<PlayerController>())
            {
                if (level == 1)
                {
                    player.GetComponent<PointsManager>().AddPoints(25);
                }
                else if (level == 2)
                {
                    player.GetComponent<PointsManager>().AddPoints(50);
                }
                else if (level == 3)
                {
                    player.GetComponent<PointsManager>().AddPoints(100);
                }
            }

            WaveManager.Instance.AddEnemyKilled();
            EnemyPool.Instance.AddToShooterPool(gameObject);
        }
    }

    protected override void AttackCrystal()
    {
        Ray ray = new Ray(attackPoint.position, shotDirection);

        RaycastHit hit;

        shot.Play();

        if (Physics.Raycast(ray, out hit, 100f, ~enemyLayer))
        {
            var selection = hit.transform;
            var hitPoint = hit.point;

            bulletTrail.enabled = true;

            bulletTrail.SetPosition(0, attackPoint.position);
            bulletTrail.SetPosition(1, hitPoint);

            if (selection.tag == "Cure")
            {
                cureTransform.gameObject.GetComponentInParent<Health>().ModifyHealth(-2);
            }
        }

        StartCoroutine(TurnOffBulletTrail());

        delayBeforeAttackReached = false;
        navMeshAgent.isStopped = false;
        AnimController.Instance.SetEnemyIsWalking(GetComponentInChildren<Animator>(), true);
    }

    protected override void AttackPlayer()
    {
        Ray ray = new Ray(attackPoint.position, shotDirection);

        RaycastHit hit;

        shot.Play();

        if (Physics.Raycast(ray, out hit, 100f, ~enemyLayer))
        {
            var selection = hit.transform;
            var hitPoint = hit.point;

            bulletTrail.enabled = true;

            if (hitPoint != null)
            {
                bulletTrail.SetPosition(0, attackPoint.position);
                bulletTrail.SetPosition(1, hitPoint);

                if (selection.tag == "Player")
                {
                    playerTransform.gameObject.GetComponentInParent<Health>().ModifyHealth(-attackDamage);
                    playerTransform.gameObject.GetComponentInParent<PlayerController>().hitSound();
                }
            }
            else
            {
                bulletTrail.SetPosition(0, attackPoint.position);
                bulletTrail.SetPosition(1, (shotDirection * 2) + attackPoint.position);
            }
        }

        StartCoroutine(TurnOffBulletTrail());

        delayBeforeAttackReached = false;
        navMeshAgent.isStopped = false;
        AnimController.Instance.SetEnemyIsWalking(GetComponentInChildren<Animator>(), true);
    }

    protected override IEnumerator DelayBeforeAttack()
    {
        if (currentAttackState == EnemyAttackStates.Player)
        {
            if ((Vector3.Distance(transform.position, playerTransform.position) < attackRange))
            {
                navMeshAgent.isStopped = true;

                AnimController.Instance.SetEnemyIsWalking(GetComponentInChildren<Animator>(), false);
                AnimController.Instance.PlayEnemyAttackAnim(GetComponentInChildren<Animator>());
            }

            yield return new WaitForSeconds(0.4f);

            shotDirection = (playerTransform.position - attackPoint.position).normalized;
        }
        else if (currentAttackState == EnemyAttackStates.Cure)
        {
            if ((Vector3.Distance(transform.position, cureTransform.position) < attackRange))
            {
                navMeshAgent.isStopped = true;

                AnimController.Instance.SetEnemyIsWalking(GetComponentInChildren<Animator>(), false);
                AnimController.Instance.PlayEnemyAttackAnim(GetComponentInChildren<Animator>());
            }

            yield return new WaitForSeconds(0.4f);

            shotDirection = (cureTransform.position - attackPoint.position).normalized;
        }

        yield return new WaitForSeconds(0.2f);

        delayBeforeAttackReached = true;
    }

    IEnumerator TurnOffBulletTrail()
    {
        yield return new WaitForSeconds(1f);

        bulletTrail.enabled = false;
    }

    public override void SetLevel(int lvl)
    {
        base.SetLevel(lvl);

        if (level == 1)
        {
            GetComponent<Health>().SetMaxHealth(35);
            GetComponent<Health>().ResetHealth();
            attackDamage = 4;

            enemyLevelBG.color = Color.yellow;
            enemyLevel.text = level.ToString();
        }
        else if (level == 2)
        {
            GetComponent<Health>().SetMaxHealth(70);
            GetComponent<Health>().ResetHealth();
            attackDamage = 9;

            enemyLevelBG.color = Color.magenta;
            enemyLevel.text = level.ToString();
        }
        else if (level == 3)
        {
            GetComponent<Health>().SetMaxHealth(130);
            GetComponent<Health>().ResetHealth();
            attackDamage = 18;

            enemyLevelBG.color = Color.red;
            enemyLevel.text = level.ToString();
        }
    }
}
