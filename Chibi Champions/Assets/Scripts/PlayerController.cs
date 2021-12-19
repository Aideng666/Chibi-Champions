using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] Transform attackPoint;
    [SerializeField] float speed = 5;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask enemyLayer;

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
        Move();

        Attack();

        controller.SimpleMove(Vector3.zero);
    }

    void Move()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            transform.rotation = Quaternion.LookRotation(moveDir);
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            AnimController.Instance.SetPlayerIsWalking(true);
        }
        else
        {
            AnimController.Instance.SetPlayerIsWalking(false);
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

            foreach(Collider enemy in hitEnemies)
            {
                Debug.Log("Hit");
            }

            AnimController.Instance.PlayAttackAnim();
        }
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
