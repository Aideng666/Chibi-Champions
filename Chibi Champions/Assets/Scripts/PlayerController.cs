using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Transform cam;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected float speed = 5;
    [SerializeField] protected float lightAttackRange;
    [SerializeField] protected float heavyAttackRange;
    [SerializeField] protected float lightAttackDamage = 10;
    [SerializeField] protected float heavyAttackDamage = 15;
    [SerializeField] protected float gravity = 1.5f;
    [SerializeField] protected float jumpPower = 10;
    [SerializeField] protected float interactDistance = 3;
    [SerializeField] protected float attackDelay = 0.75f;
    [SerializeField] protected TextMeshProUGUI interactText;

    protected CharacterController controller;

    protected float horizontalInput = 0f;
    protected float verticalInput = 0f;
    protected Vector3 direction;
    protected Vector3 moveDir;
    protected bool isJumping;

    protected float timeToNextAttack = 0;

    protected bool canInteract;

    // Start is called before the first frame update
    protected void Start()
    {
        controller = GetComponent<CharacterController>();

        interactText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected void Update()
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

    protected IEnumerator Jump()
    {
        moveDir.y = jumpPower;

        isJumping = true;

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

    protected virtual void Attack()
    {

    }

    protected bool CanAttack()
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

                if (selection.parent.tag == "Tower")
                {
                    TowerMenu.Instance.SetMenuState(MenuState.Upgrade);
                    TowerMenu.Instance.SetTower(selection.parent);
                }
                else
                {
                    TowerMenu.Instance.SetMenuState(MenuState.Buy);
                    TowerMenu.Instance.SetPlatform(selection);
                }
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

        Gizmos.DrawWireSphere(attackPoint.position, lightAttackRange);
    }
}
