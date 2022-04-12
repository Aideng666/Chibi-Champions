using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drumstick : PlayerController
{
    bool groundPoundActivated;
    bool speedParticleActivated;
    ParticleSystem speedParticle;

    [SerializeField] AudioSource fall;
    [SerializeField] AudioSource land;
    [SerializeField] AudioSource wack;
    [SerializeField] TrailRenderer swordTrail;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        swordTrail.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (FindObjectOfType<AudioManager>().isMute() == true)
        //{
        //    fall.mute = true;
        //    land.mute = true;
        //    wack.mute = true;
        //}
        //else
        //{
        //    fall.mute = false;
        //    land.mute = false;
        //    wack.mute = false;
        //}
        //fall.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
        //land.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
        //wack.volume = FindObjectOfType<AudioManager>().GetSFXVolume();

        if (groundPoundActivated && controller.isGrounded)
        {
            GroundPoundAttack();

            Destroy(speedParticle.gameObject);

            fall.Stop();
            land.Play();
        }

        if (groundPoundActivated && isJumping && controller.velocity.y < 0 && !speedParticleActivated)
        {
            speedParticle = ParticleManager.Instance.SpawnParticle(ParticleTypes.Speed, transform.position).GetComponent<ParticleSystem>();

            speedParticleActivated = true;
        }

        if (speedParticle != null)
        {
            speedParticle.transform.position = transform.position;
        }


        if (isPlayerCharacter)
        {
            base.Update();

            AbilityCooldown(groundPoundActivated);
        }
    }

    protected override void Attack()
    {
        if (!CanvasManager.isGamePaused && !CanvasManager.isMultiplayerPaused)
        {
            if (Input.GetMouseButton(0) && CanLightAttack())
            {
                if (FindObjectOfType<UDPClient>() != null)
                {
                    UDPClient.Instance.SendPlayerUpdates(ActionTypes.Attack, GetCharacterNameEnum());
                }

                AnimController.Instance.PlayPlayerAttackAnim(GetComponentInChildren<Animator>());

                wack.Play();

                StartCoroutine(DelayBeforeAttack());
            }
            if (Input.GetMouseButtonDown(1) && CanHeavyAttack()/* && controller.isGrounded*/)
            {
                if (FindObjectOfType<UDPClient>() != null)
                {
                    UDPClient.Instance.SendPlayerUpdates(ActionTypes.Ability, GetCharacterNameEnum());
                }

                StartCoroutine(GroundPoundJump());

                ParticleManager.Instance.SpawnParticle(ParticleTypes.HighJump, transform.position);

                groundPoundActivated = true;

                fall.Play();

                AnimController.Instance.PlayPlayerAbilityAnim(GetComponentInChildren<Animator>());
            }
        }     
    }

    public override void ReceiveAttackTrigger()
    {
        AnimController.Instance.PlayPlayerAttackAnim(GetComponentInChildren<Animator>());

        wack.Play();

        StartCoroutine(DelayBeforeAttack());
    }

    public override void ReceiveAbilityTrigger()
    {
        StartCoroutine(GroundPoundJump());

        ParticleManager.Instance.SpawnParticle(ParticleTypes.HighJump, transform.position);

        groundPoundActivated = true;

        fall.Play();

        AnimController.Instance.PlayPlayerAbilityAnim(GetComponentInChildren<Animator>());
    }

    void GroundPoundAttack()
    {
        ParticleManager.Instance.SpawnParticle(ParticleTypes.GroundPound, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), heavyAttackRange);

        groundPoundActivated = false;

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, heavyAttackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-abilityDamage);
                enemy.GetComponentInParent<Enemy>().Knockback(35, transform);
                enemy.GetComponentInParent<Enemy>().SetLastHit(this);
            }
        }

        speedParticleActivated = false;
    }

    protected IEnumerator GroundPoundJump()
    {
        moveDir.y = jumpPower * 5;

        isJumping = true;

        float elasped = 0f;
        float totalJumpTime = 0.6f;
        float totalUpTime = 0.3f;
        float totalStallTime = 0.4f;

        while (elasped < totalUpTime)
        {
            elasped += Time.deltaTime;
            moveDir.y = Mathf.Lerp(jumpPower, 0.1f, elasped / totalUpTime);

            controller.Move(moveDir * speed * Time.deltaTime);

            yield return null;
        }

        elasped = 0;

        while (elasped < totalStallTime)
        {
            elasped += Time.deltaTime;
            moveDir.y = Mathf.Lerp(0.1f, 0, elasped / totalStallTime);

            controller.Move(moveDir * speed * Time.deltaTime);

            yield return null;
        }

        elasped = 0;

        while (elasped < totalJumpTime)
        {
            elasped += Time.deltaTime;
            moveDir.y = Mathf.Lerp(-1, -gravity * 5, elasped / totalJumpTime);

            controller.Move(moveDir * speed * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator DelayBeforeAttack()
    {
        yield return new WaitForSeconds(0.1f);

        swordTrail.emitting = true;

        yield return new WaitForSeconds(0.1f);

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, lightAttackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-lightAttackDamage);
                enemy.GetComponentInParent<Enemy>().Knockback(20, transform);
                enemy.GetComponentInParent<Enemy>().SetLastHit(this);
                enemy.GetComponentInParent<Enemy>().HitSound();

                ParticleManager.Instance.SpawnParticle(ParticleTypes.Hurt, enemy.transform.position);
            }
        }

        swordTrail.emitting = false;
    }
}
