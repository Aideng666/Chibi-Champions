using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisBomb : Tower
{
    [SerializeField] GameObject tennisBallPrefab;
    [SerializeField] float launchForce;
    [SerializeField] float fuseDuration;

    [SerializeField] AudioSource serve;


    // Update is called once per frame
    void Update()
    {
        UpdateView();

        if (targetEnemy == null)
        {
            return;
        }

        transform.LookAt(new Vector3(targetEnemy.transform.position.x, 0.5f, targetEnemy.transform.position.z));

        if (CanAttack())
        {
            Attack(targetEnemy);
        }
    }

    protected override void Attack(GameObject enemy = null)
    {
        serve.Play();
        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        direction.y = 1;

        direction = direction.normalized;

        var tennisBall = Instantiate(tennisBallPrefab, firePoint.position, Quaternion.identity);

        tennisBall.GetComponent<Rigidbody>().AddForce(direction * launchForce, ForceMode.Impulse);
        tennisBall.GetComponent<TennisBall>().SetTower(this);
        tennisBall.GetComponent<TennisBall>().SetFuseDuration(fuseDuration);
    }

    public override void Upgrade()
    {
        if (towerLevel == 1)
        {
            fuseDuration *= 1.5f;
        }
        else if (towerLevel == 2)
        {
            towerDamage *= 1.5f;
        }
        else if (towerLevel == 3)
        {
            attackDelay /= 1.5f;
        }
        else
        {
            print("Tower is Max Level");
            return;
        }

        base.Upgrade();
    }
}
