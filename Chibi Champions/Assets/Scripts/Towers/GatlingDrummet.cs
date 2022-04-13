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
    private void Start()
    {
        shot.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
    }
    void Update()
    {
        if (FindObjectOfType<AudioManager>().dirtyGat)
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
            FindObjectOfType<AudioManager>().dirtyGat = false;

        }
        UpdateView();

        if (targetEnemy == null)
        {
            AnimController.Instance.SetGatlingDrummetFiring(GetComponentInChildren<Animator>(), false);

            shot.Stop();

            return;
        }

        partToRotate.transform.LookAt(new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y - 1.5f, targetEnemy.transform.position.z));

        if (CanAttack())
        {
            Attack(targetEnemy);          
        }
    }

    protected override void Attack(GameObject enemy = null)
    {
        StartCoroutine(FireSound());

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
