using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisBomb : Tower
{
    [SerializeField] GameObject tennisBallPrefab;
    [SerializeField] float launchForce;
    [SerializeField] float fuseDuration;
    [SerializeField] AudioSource shot;

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<AudioManager>().isMute() == true)
        {
            shot.mute = true;
        }
        else
        {
            shot.mute = false;
        }

        shot.volume = FindObjectOfType<AudioManager>().GetSFXVolume();

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
        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        direction.y = 1;

        direction = direction.normalized;

        AnimController.Instance.PlayTowerShootAnim(GetComponentInChildren<Animator>());

        var tennisBall = Instantiate(tennisBallPrefab, firePoint.position, Quaternion.identity);

        tennisBall.GetComponent<Rigidbody>().AddForce(direction * launchForce, ForceMode.Impulse);
        tennisBall.GetComponent<TennisBall>().SetTower(this);
        tennisBall.GetComponent<TennisBall>().SetFuseDuration(fuseDuration);
        shot.Play();
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
