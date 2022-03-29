using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WebShooter : Tower
{
    [SerializeField] float webSpeed;
    [SerializeField] float slowDuration;
    [SerializeField] GameObject webPrefab;

    [SerializeField] AudioSource spidy;

    bool dealDamage;

    // Update is called once per frame
    void Update()
    {
        UpdateView();

        if (targetEnemy == null)
        {
            return;
        }

        transform.LookAt(targetEnemy.transform.position);

        if (CanAttack())
        {
            Attack(targetEnemy);
        }
    }

    protected override void Attack(GameObject enemy = null)
    {
        spidy.Play();
        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        var web = Instantiate(webPrefab, firePoint.position, Quaternion.identity);

        web.transform.LookAt(enemy.transform);
        web.GetComponentInChildren<Rigidbody>().velocity = direction * webSpeed;
        web.GetComponentInChildren<Web>().SetTower(this);
        web.GetComponentInChildren<Web>().SetDealDamage(dealDamage);

        Destroy(web, 3);
    }

    public void ApplySlowEffect(GameObject enemy)
    {
        StartCoroutine(SlowDuration(enemy));
    }

    IEnumerator SlowDuration(GameObject enemy)
    {
        enemy.GetComponentInParent<NavMeshAgent>().speed = 1;

        yield return new WaitForSeconds(slowDuration);

        enemy.GetComponentInParent<NavMeshAgent>().speed = enemy.GetComponentInParent<Enemy>().GetDefaultSpeed();
    }

    public override void Upgrade()
    {
        if (towerLevel == 1)
        {
            attackRange += 5f;
        }
        else if (towerLevel == 2)
        {
            slowDuration *= 1.5f;
        }
        else if (towerLevel == 3)
        {
            dealDamage = true;
        }
        else
        {
            print("Tower is Max Level");
            return;
        }

        base.Upgrade();
    }
}
