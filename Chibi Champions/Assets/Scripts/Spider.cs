using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float applyEffectDistance = 1.8f;
    Tower tower;
    float tickDelay;
    SpiderStates currentState = SpiderStates.noEnemySighted;
    GameObject targetEnemy;

    [SerializeField] AudioSource walk;
    [SerializeField] AudioSource atk;

    Vector3 moveDirection;
    bool isMoving;

    float timeToMovementChange = 0;
    float movementChangeDelay = 0.5f;

    int effectedEnemies = 0;

    private enum SpiderStates
    {
        enemySighted,
        noEnemySighted
    }
    private void Start()
    {
        walk.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
        atk.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<AudioManager>().dirtySpi)
        {
            if (FindObjectOfType<AudioManager>().isMute() == true)
            {
                walk.mute = true;
                atk.mute = true;
            }
            else
            {
                walk.mute = false;
                atk.mute = false;
            }

            walk.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
            atk.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
            FindObjectOfType<AudioManager>().dirtySpi = false;

        }


        Collider[] EnemiesInView = Physics.OverlapSphere(transform.position, tower.GetRange(), tower.GetEnemyLayer());

        if (EnemiesInView == null || EnemiesInView.Length < 1)
        {
            currentState = SpiderStates.noEnemySighted;
            moveSpeed = 2;
        }
        else
        {
            foreach (Collider enemy in EnemiesInView)
            {
                if (enemy.GetComponentInParent<Enemy>().GetCurrentEffect() == Effects.Spider)
                {
                    effectedEnemies++;
                }
            }

            if (effectedEnemies == EnemiesInView.Length)
            {
                currentState = SpiderStates.noEnemySighted;
                moveSpeed = 2;
            }
            else
            {
                currentState = SpiderStates.enemySighted;
                moveSpeed = 5;
            }

            effectedEnemies = 0;
        }


        if (currentState == SpiderStates.enemySighted)
        {
            Collider selectedEnemy = EnemiesInView[0];

            Collider currentEnemyCheck = EnemiesInView[0];

            for (int i = 0; i < EnemiesInView.Length; i++)
            {
                currentEnemyCheck = EnemiesInView[i];

                if (Vector3.Distance(currentEnemyCheck.transform.position, transform.position) < Vector3.Distance(selectedEnemy.transform.position, transform.position))
                {
                    if (currentEnemyCheck.GetComponentInParent<Enemy>().GetCurrentEffect() == Effects.None)
                    {
                        selectedEnemy = currentEnemyCheck;
                    }
                }
            }

            targetEnemy = selectedEnemy.gameObject;

            ChaseEnemy(targetEnemy);
        }
        else if (currentState == SpiderStates.noEnemySighted)
        {
            SearchForEnemy();
        }
    }

    void ChaseEnemy(GameObject enemy)
    {
        //walk.Play();

        transform.LookAt(enemy.transform);

        moveDirection = (enemy.transform.position - transform.position).normalized;

        moveDirection.y = 0;

        moveDirection = moveDirection.normalized;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, enemy.transform.position) < applyEffectDistance)
        {
            ApplyEffect();
            atk.Play();
        }
    }

    void SearchForEnemy()
    {
        if (ShouldChangeMovement())
        {
            isMoving = !isMoving;
        }

        if (!isMoving)
        {
            moveDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        }

        if (isMoving)
        {
            transform.LookAt(transform.position + (moveDirection * moveSpeed * Time.deltaTime));

            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            walk.Play();

        }
    }

    bool ShouldChangeMovement()
    {
        if (timeToMovementChange < Time.realtimeSinceStartup)
        {
            timeToMovementChange = Time.realtimeSinceStartup + movementChangeDelay;
            return true;
        }

        return false;
    }

    void ApplyEffect()
    {
        targetEnemy.GetComponentInParent<Enemy>().SetEffect(Effects.Spider);
        targetEnemy.GetComponentInParent<Enemy>().SetEffectTickDelay(tickDelay);

        tower.GetComponent<SpiderHouse>().RemoveSpider();

        Destroy(gameObject);
    }

    public void SetTower(Tower t)
    {
        print($"Tower Set: {t.gameObject.name}");

        tower = t;
    }

    public void SetTickDelay(float delay)
    {
        tickDelay = delay;
    }
}
