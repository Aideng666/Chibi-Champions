using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    Tower tower;
    float tickDelay;
    SpiderStates currentState = SpiderStates.noEnemySighted;
    GameObject targetEnemy;

    bool switchRandomDirection = true;

    Vector3 moveDirection;
    bool isMoving;

    private enum SpiderStates
    {
        enemySighted,
        noEnemySighted
    }


    // Update is called once per frame
    void Update()
    {
        Collider[] EnemiesInView = Physics.OverlapSphere(transform.position, tower.GetRange(), tower.GetEnemyLayer());

        if (EnemiesInView == null || EnemiesInView.Length < 1)
        {
            currentState = SpiderStates.noEnemySighted;
        }
        else
        {
            currentState = SpiderStates.enemySighted;
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
                    selectedEnemy = currentEnemyCheck;
                }
            }

            targetEnemy = selectedEnemy.gameObject;

            ChaseEnemy(targetEnemy);
        }
        else if (currentState == SpiderStates.noEnemySighted)
        {
            SearchForEnemy(); 
        }

        print("Move Direction: " + moveDirection);

    }

    void ChaseEnemy(GameObject enemy)
    {
        moveDirection = (enemy.transform.position - transform.position).normalized;

        moveDirection.y = 0;

        moveDirection = moveDirection.normalized;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void SearchForEnemy()
    {
        if (switchRandomDirection)
        {
            moveDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        }

        if (isMoving)
        {
            StartCoroutine(SearchMovement());
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            StartCoroutine(SearchMovement());
        }
    }

    IEnumerator SearchMovement()
    {
        yield return new WaitForSeconds(0.5f);

        isMoving = !isMoving;
    }

    public void SetTower(Tower t)
    {
        tower = t;
    }

    public void SetTickDelay(float delay)
    {
        tickDelay = delay;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
