using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingDrummet : Tower
{
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject partToRotate;

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

        shot.maxDistance = attackRange;
        shot.minDistance = shot.maxDistance - 2;
        UpdateView();

        if (targetEnemy == null)
        {
            AnimController.Instance.SetGatlingDrummetFiring(GetComponentInChildren<Animator>(), false);

            //shot.Stop();

            return;
        }

        partToRotate.transform.LookAt(new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y - 1.5f, targetEnemy.transform.position.z));

        if (CanAttack())
        {
            Attack(targetEnemy);

            Debug.Log(shot + " gun");
            //shot.Play();

            StartCoroutine(FireSound());
        }
    }

    protected override void Attack(GameObject enemy = null)
    {
        AnimController.Instance.SetGatlingDrummetFiring(GetComponentInChildren<Animator>(), true);

        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        //var feather = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        var feather = ProjectilePool.Instance.GetFeatherFromPool(firePoint.position);

        //feather.transform.position = firePoint.position;

        feather.transform.LookAt(enemy.transform);
        feather.GetComponentInChildren<Rigidbody>().velocity = direction * bulletSpeed;
        feather.GetComponentInChildren<Feather>().SetTower(this);

        //Destroy(feather, 3);
    }

    IEnumerator FireSound()
    {
        Debug.Log("woohoo");
        shot.Play();
        yield return new WaitForSeconds(0.5f);

    }
    public override void Upgrade()
    {
        if (towerLevel == 1)
        {
            attackRange += 5;
        }
        else if (towerLevel == 2)
        {
            attackDelay /= 1.5f;
        }
        else if (towerLevel == 3)
        {
            towerDamage *= 1.5f;
        }
        else
        {
            print("Tower is Max Level");
            return;
        }

        base.Upgrade();
    }
}
