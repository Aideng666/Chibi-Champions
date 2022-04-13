using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WebShooter : Tower
{
    [SerializeField] float webSpeed;
    [SerializeField] float slowDuration;
    [SerializeField] GameObject webPrefab;
    [SerializeField] GameObject partToRotate;
    [SerializeField] AudioSource shot;

    private void Start()
    {
        base.StartTower();

        shot.volume = AudioManager.Instance.GetSFXVolume();
    }
    // Update is called once per frame
    void Update()
    {
        if (AudioManager.Instance.dirtyWeb) { 
            if (AudioManager.Instance.isMute() == true)
            {
                shot.mute = true;
            }
            else
            {
                shot.mute = false;
            }

            shot.volume = AudioManager.Instance.GetSFXVolume();
            AudioManager.Instance.dirtyWeb = false;

        }



        UpdateView();

        if (targetEnemy == null)
        {
            return;
        }

        //transform.LookAt(targetEnemy.transform.position);
       // partToRotate.transform.LookAt(new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y - 1.5f, targetEnemy.transform.position.z));
        partToRotate.transform.LookAt(targetEnemy.transform.position);

        if (CanAttack())
        {
            Attack(targetEnemy);
        }
    }

    protected override void Attack(GameObject enemy = null)
    {
        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        AnimController.Instance.PlayTowerShootAnim(GetComponentInChildren<Animator>());

        var web = Instantiate(webPrefab, firePoint.position, Quaternion.identity);

        web.transform.LookAt(enemy.transform);
        web.GetComponentInChildren<Rigidbody>().velocity = direction * webSpeed;
        web.GetComponentInChildren<Web>().SetTower(this);
        shot.Play();

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

        if (enemy.activeSelf)
        { 
            enemy.GetComponentInParent<NavMeshAgent>().speed = enemy.GetComponentInParent<Enemy>().GetDefaultSpeed();
        }
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
            towerDamage *= 2;
        }
        else
        {
            print("Tower is Max Level");
            return;
        }

        base.Upgrade();
    }
}
