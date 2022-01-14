using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drumstick : PlayerController
{
    bool groundPoundActivated;

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
        }
    }

    protected override void Attack()
    {
        if (Input.GetMouseButtonDown(0) && CanAttack())
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, lightAttackRange, enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.tag == "Enemy")
                {
                    enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-lightAttackDamage);
                    enemy.GetComponentInParent<Enemy>().Knockback(20);
                }
            }

            AnimController.Instance.PlayPlayerAttackAnim();
        }
        if (Input.GetMouseButtonDown(1) && CanAttack())
        {
            StartCoroutine(Jump());

            groundPoundActivated = true;
        }
    }

    void GroundPoundAttack()
    {
        groundPoundActivated = false;

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, heavyAttackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                print("Hit One Enemy");
                enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-heavyAttackDamage);
                enemy.GetComponentInParent<Enemy>().Knockback(35);
            }
        }

    }
}
