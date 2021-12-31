using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Transform cam;
    [SerializeField] Transform attackPoint;
    [SerializeField] float speed = 5;
    [SerializeField] float attackRange;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float jumpPower = 10;
    [SerializeField] float interactDistance = 3;
    [SerializeField] float attackDelay = 0.75f;
    [SerializeField] TextMeshProUGUI interactText;

    CharacterController controller;

    float horizontalInput = 0f;
    float verticalInput = 0f;
    Vector3 direction;
    Vector3 moveDir;
    bool isJumping;

    float timeToNextAttack = 0;

    bool canInteract;
    bool menuOpen;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        interactText.gameObject.SetActive(false);
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

        CheckRaycastSelection();


        if (canInteract && Input.GetKeyDown(KeyCode.E) && !CanvasManager.Instance.IsTowerMenuOpen())
        {
            CanvasManager.Instance.OpenTowerMenu();
        }
        else if (CanvasManager.Instance.IsTowerMenuOpen() && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)))
        {
            CanvasManager.Instance.CloseTowerMenu();
        }

    }

    void Move()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        transform.rotation = cam.rotation;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            moveDir.y = jumpPower;

            isJumping = true;

            StartCoroutine(Jump());
        }

        if (!isJumping)
        {
            moveDir.y -= gravity;
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

        if (!isJumping)
        {
            controller.Move(moveDir * speed * Time.deltaTime);
        }
    }

    IEnumerator Jump()
    {
        float elasped = 0f;
        float totalJumpTime = 0.5f;

        while (elasped < totalJumpTime)
        {
            elasped += Time.deltaTime;
            moveDir.y =  Mathf.Lerp(jumpPower, -gravity, elasped / totalJumpTime);

            controller.Move(moveDir * speed * Time.deltaTime);

            yield return null;
        }

        isJumping = false;
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && CanAttack())
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

            foreach(Collider enemy in hitEnemies)
            {
                enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-10);
            }

            AnimController.Instance.PlayPlayerAttackAnim();
        }
    }

    bool CanAttack()
    {
        if (timeToNextAttack < Time.realtimeSinceStartup)
        {
            timeToNextAttack = Time.realtimeSinceStartup + attackDelay;
            return true;
        }

        return false;
    }

    void GameOver()
    {
        SceneManager.LoadScene("Lose");
    }

    void CheckRaycastSelection()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;

            if (selection != null && selection.tag == "Interactable" && Vector3.Distance(transform.position, selection.position) < interactDistance)
            {
                canInteract = true;
                interactText.gameObject.SetActive(true);
                TowerMenu.Instance.SetPlatform(selection);
            }
            else
            {
                canInteract = false;
                interactText.gameObject.SetActive(false);
            }
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
