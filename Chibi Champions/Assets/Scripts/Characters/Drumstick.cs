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
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

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
            fall.Play();

        }

        if (speedParticle != null)
        {
            speedParticle.transform.position = transform.position;
        }

    }

    protected override void Attack()
    {
        if (Input.GetMouseButtonDown(0) && CanLightAttack())
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, lightAttackRange, enemyLayer);
            wack.Play();
            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.tag == "Enemy")
                {
                    enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-lightAttackDamage);
                    enemy.GetComponentInParent<Enemy>().Knockback(20, transform);
                    enemy.GetComponentInParent<Enemy>().SetLastHit(this);
                    //GetComponent<PointsManager>().AddPoints(20);

                    ParticleManager.Instance.SpawnParticle(ParticleTypes.Hurt, enemy.transform.position);
                }
            }

            AnimController.Instance.PlayPlayerAttackAnim();
        }
        if (Input.GetMouseButtonDown(1) && CanHeavyAttack())
        {
            StartCoroutine(Jump());

            ParticleManager.Instance.SpawnParticle(ParticleTypes.HighJump, transform.position);

            groundPoundActivated = true;
        }
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
                enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-heavyAttackDamage);
                enemy.GetComponentInParent<Enemy>().Knockback(35, transform);
                enemy.GetComponentInParent<Enemy>().SetLastHit(this);
                //GetComponent<PointsManager>().AddPoints(20);
            }
        }

        speedParticleActivated = false;
    }
}
