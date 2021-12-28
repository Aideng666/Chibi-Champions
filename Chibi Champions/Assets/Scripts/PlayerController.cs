using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Transform cam;
    [SerializeField] Transform attackPoint;
    [SerializeField] float speed = 5;
    [SerializeField] float attackRange;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float jumpPower = 10;

    CharacterController controller;

    float horizontalInput = 0f;
    float verticalInput = 0f;
    Vector3 direction;
    Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Health>().GetCurrentHealth() <= 0)
        {
            GameOver();
        }

        Move();

        Attack();

        //controller.SimpleMove(Vector3.zero);
    }

    void Move()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        transform.rotation = cam.rotation;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        moveDir.y -= gravity;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveDir.y = jumpPower;
        }


        if (direction.magnitude >= 0.1f)
        {
            AnimController.Instance.SetPlayerIsWalking(true);
        }
        else
        {
            moveDir = new Vector3(0, moveDir.y, 0);
            AnimController.Instance.SetPlayerIsWalking(false);
        }

        controller.Move(moveDir * speed * Time.deltaTime);
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

            foreach(Collider enemy in hitEnemies)
            {
                enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-10);
            }

            AnimController.Instance.PlayPlayerAttackAnim();
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("Lose");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
