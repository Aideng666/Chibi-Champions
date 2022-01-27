using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolfe : PlayerController
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        if (Input.GetMouseButtonDown(0) && CanLightAttack())
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, lightAttackRange, enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.tag == "Enemy")
                {
                    enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-lightAttackDamage);
                    enemy.GetComponentInParent<Enemy>().Knockback(10);
                    enemy.GetComponentInParent<Enemy>().SetLastHit(this);
                    GetComponent<PointsManager>().AddPoints(20);

                }
            }

            StartCoroutine(WaitForSecondSwipe());

            //AnimController.Instance.PlayPlayerAttackAnim();
        }
        if (Input.GetMouseButtonDown(1) && CanHeavyAttack())
        {

        }
    }

    void SecondSwipeAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, lightAttackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-lightAttackDamage);
                enemy.GetComponentInParent<Enemy>().Knockback(10);
                enemy.GetComponentInParent<Enemy>().SetLastHit(this);
                GetComponent<PointsManager>().AddPoints(20);

            }
        }
    }

    IEnumerator WaitForSecondSwipe()
    {
        yield return new WaitForSeconds(0.2f);

        SecondSwipeAttack();
    }
}
