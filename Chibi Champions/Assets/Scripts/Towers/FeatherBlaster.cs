using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherBlaster : Tower
{
    [SerializeField] float featherSpeed;
    [SerializeField] GameObject featherPrefab;
    [SerializeField] GameObject partToRotate;

    [SerializeField] AudioSource shot;

    bool shootTwoShots;

    private void Start()
    {
        shot.volume = FindObjectOfType<AudioManager>().GetSFXVolume();

    }
    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<AudioManager>().dirtyBla)
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
            FindObjectOfType<AudioManager>().dirtyBla = false;

        }


        UpdateView();

        if (targetEnemy == null)
        {
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
        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        //var feather = Instantiate(featherPrefab, firePoint.position, Quaternion.identity);
        var feather = ProjectilePool.Instance.GetFeatherFromPool(firePoint.position);

        //feather.transform.position = firePoint.position;

        feather.transform.LookAt(enemy.transform);
        feather.GetComponentInChildren<Rigidbody>().velocity = direction * featherSpeed;
        feather.GetComponentInChildren<Feather>().SetTower(this);

        AnimController.Instance.PlayTowerShootAnim(GetComponentInChildren<Animator>());

        shot.Play();

        if (shootTwoShots)
        {
            SecondShot(enemy);
        }
    }

    void SecondShot(GameObject enemy = null)
    {
        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        //var feather = Instantiate(featherPrefab, new Vector3(firePoint.position.x, firePoint.position.y + 0.5f, firePoint.position.z), Quaternion.identity);
        var feather = ProjectilePool.Instance.GetFeatherFromPool(new Vector3(firePoint.position.x, firePoint.position.y + 0.5f, firePoint.position.z));

        //feather.transform.position = new Vector3(firePoint.position.x, firePoint.position.y + 0.5f, firePoint.position.z);

        feather.transform.LookAt(enemy.transform);
        feather.GetComponentInChildren<Rigidbody>().velocity = direction * featherSpeed;
        feather.GetComponentInChildren<Feather>().SetTower(this);

        //Destroy(feather, 3);
    }

    public override void Upgrade()
    {
        if (towerLevel == 1)
        {
            attackRange += 5f;
        }
        else if (towerLevel == 2)
        {
            towerDamage *= 1.5f;
        }
        else if (towerLevel == 3)
        {
            shootTwoShots = true;
        }
        else
        {
            print("Tower is Max Level");
            return;
        }

        base.Upgrade();
    }
}
