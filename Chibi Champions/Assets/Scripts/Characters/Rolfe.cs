using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolfe : PlayerController
{
    [SerializeField] GameObject beaconPrefab;
    [SerializeField] int maxBeacons = 2;

    int currentBeacons = 0;

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
                    enemy.GetComponentInParent<Enemy>().Knockback(20, transform);
                    enemy.GetComponentInParent<Enemy>().SetLastHit(this);
                    GetComponent<PointsManager>().AddPoints(20);

                    ParticleManager.Instance.SpawnParticle(ParticleTypes.Hurt, enemy.transform.position);
                }
            }

            StartCoroutine(WaitForSecondSwipe());

            //AnimController.Instance.PlayPlayerAttackAnim();
        }
        if (Input.GetMouseButtonDown(1) && CanHeavyAttack() && currentBeacons < maxBeacons)
        {
            var beacon = Instantiate(beaconPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            currentBeacons++;
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
                enemy.GetComponentInParent<Enemy>().Knockback(20, transform);
                enemy.GetComponentInParent<Enemy>().SetLastHit(this);
                GetComponent<PointsManager>().AddPoints(20);

                ParticleManager.Instance.SpawnParticle(ParticleTypes.Hurt, enemy.transform.position);
            }
        }
    }

    public void RemoveBeacon()
    {
        if (currentBeacons > 0)
        {
            currentBeacons--;
        }
    }

    IEnumerator WaitForSecondSwipe()
    {
        yield return new WaitForSeconds(0.4f);

        SecondSwipeAttack();
    }
}
