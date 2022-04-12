using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InkBomber : Tower
{
    [SerializeField] GameObject inkPrefab;
    [SerializeField] GameObject partToRotate;
    [SerializeField] float stunDuration;
    [SerializeField] float inkSpeed;

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
        //shot.maxDistance = attackRange;
        //shot.minDistance = shot.maxDistance - 2;
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
        shot.Play();
        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        var ink = Instantiate(inkPrefab, firePoint.position, Quaternion.identity);

        ink.GetComponentInChildren<Rigidbody>().velocity = direction * inkSpeed;
        ink.GetComponentInChildren<InkBlob>().SetTower(this);

        AnimController.Instance.PlayTowerShootAnim(GetComponentInChildren<Animator>());

        Destroy(ink, 3);
    }

    public void ApplyStunEffect(GameObject enemy)
    {
        StartCoroutine(StunDuration(enemy));
    }

    IEnumerator StunDuration(GameObject enemy)
    {
        enemy.GetComponentInParent<NavMeshAgent>().speed = 0;

        yield return new WaitForSeconds(stunDuration);

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
            stunDuration *= 1.5f;
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
