using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected string characterName;
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
    [SerializeField] protected float lightAttackDelay = 0.75f;
    [SerializeField] protected float heavyAttackDelay = 0.75f;
    [SerializeField] protected float deathTimer = 5;
    [SerializeField] protected float effectSpan = 15;
    [SerializeField] protected TextMeshProUGUI interactText;
    [SerializeField] protected GameObject cameraLookAt;
    [SerializeField] protected GameObject[] towers = new GameObject[3];
    [SerializeField] protected Transform respawnLocation;

    [SerializeField] AudioSource jump;
    [SerializeField] AudioSource dead;

    [SerializeField] protected Image abilityImage;
    [SerializeField] protected Image abilityImageMain;
    protected bool isCooldown = false;
    [SerializeField] protected CharacterDatabase characterDB;
    Character character;

    protected CharacterController controller;
    protected CinemachineVirtualCamera thirdPersonCam;
    protected Transform rayCastSelection;

    protected float horizontalInput = 0f;
    protected float verticalInput = 0f;
    protected Vector3 direction;
    protected Vector3 moveDir;
    protected bool isJumping;

    protected float timeToNextLightAttack = 0;
    protected float timeToNextHeavyAttack = 0;

    protected bool canInteract;

    protected bool effectApplied;
    protected bool isAlive = true;

    protected bool isPlayerCharacter;

    int sporeLevel = 1;
    Effects currentEffect = Effects.None;

    // Start is called before the first frame update
    protected void Start()
    {
        controller = GetComponent<CharacterController>();

        interactText.gameObject.SetActive(false);

        thirdPersonCam = FindObjectOfType<CinemachineVirtualCamera>();

        abilityImage.fillAmount = 0;

        character = characterDB.GetCharacter(PlayerPrefs.GetInt("CharacterIndex"));
        abilityImage.sprite = character.abilitySprites[1];
        abilityImageMain.sprite = character.abilitySprites[1];
    }

    // Update is called once per frame
    protected void Update()
    {
        if (isPlayerCharacter)
        {
            thirdPersonCam.LookAt = cameraLookAt.transform;
            thirdPersonCam.Follow = cameraLookAt.transform;

            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {            
                if (CanvasManager.isGamePaused)
                {
                    CanvasManager.Instance.Resume();
                }
                else
                {
                    CanvasManager.Instance.Pause();
                }
            }        

            if (gameObject.GetComponent<Health>().GetCurrentHealth() <= 0 && isAlive)
            {
                Die();
            }

            if (isAlive)
            {
                ApplyEffect();

                Move();

                Attack();

                CheckRaycastSelection();

                if (canInteract && Input.GetKeyDown(KeyCode.E) && !CanvasManager.Instance.IsTowerMenuOpen())
                {
                    TowerMenu.Instance.SetPlayer(this);
                    CanvasManager.Instance.OpenTowerMenu();
                }
                else if (CanvasManager.Instance.IsTowerMenuOpen() && Input.GetKeyDown(KeyCode.E))
                {
                    CanvasManager.Instance.CloseTowerMenu();
                }

                if (CanvasManager.Instance.IsTowerMenuOpen() || CanvasManager.isGamePaused)
                {
                    CameraLock(true);
                    CanvasManager.Instance.RemoveCursorLock();
                }
                else
                {
                    CameraLock(false);
                    CanvasManager.Instance.ApplyCursorLock();
                }
            }        
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
            AnimController.Instance.PlayPlayerJumpAnim(GetComponentInChildren<Animator>(), false);

            StartCoroutine(Jump());

            jump.Play();
        }

        if (isJumping && controller.isGrounded)
        {
            print("Jumping Done");
            isJumping = false;
        }

        if (!isJumping)
        {
            moveDir.y -= gravity;
        }

        if (direction.magnitude >= 0.1f)
        {
            //AnimController.Instance.SetPlayerIsWalking(true);
        }
        else
        {
            moveDir = new Vector3(0, moveDir.y, 0);
            //AnimController.Instance.SetPlayerIsWalking(false);
        }

        if (!isJumping)
        {
            controller.Move(moveDir * speed * Time.deltaTime);
        }

        if (!CanvasManager.isGamePaused)
        {
            if (verticalInput > 0)
            {
                AnimController.Instance.SetPlayerWalking(GetComponentInChildren<Animator>(), true, true, false);
                AnimController.Instance.SetPlayerWalking(GetComponentInChildren<Animator>(), false, false, false);
            }
            else if (verticalInput < 0)
            {
                AnimController.Instance.SetPlayerWalking(GetComponentInChildren<Animator>(), true, false, false);
                AnimController.Instance.SetPlayerWalking(GetComponentInChildren<Animator>(), false, true, false);
            }
            else
            {
                AnimController.Instance.SetPlayerWalking(GetComponentInChildren<Animator>(), false, true, false);
                AnimController.Instance.SetPlayerWalking(GetComponentInChildren<Animator>(), false, false, false);
            }

            if (horizontalInput > 0)
            {
                AnimController.Instance.SetPlayerStrafing(GetComponentInChildren<Animator>(), true, 1, false);
                AnimController.Instance.SetPlayerStrafing(GetComponentInChildren<Animator>(), false, 0, false);
            }
            else if (horizontalInput < 0)
            {
                AnimController.Instance.SetPlayerStrafing(GetComponentInChildren<Animator>(), true, 0, false);
                AnimController.Instance.SetPlayerStrafing(GetComponentInChildren<Animator>(), false, 1, false);
            }
            else
            {
                AnimController.Instance.SetPlayerStrafing(GetComponentInChildren<Animator>(), false, 0, false);
                AnimController.Instance.SetPlayerStrafing(GetComponentInChildren<Animator>(), false, 1, false);
            }
        }       
    }

    void ApplyEffect()
    {
        if (currentEffect == Effects.None)
        {
            effectApplied = false;

            return;
        }
        else if (currentEffect == Effects.PED)
        {
            if (!effectApplied)
            {
                if (sporeLevel == 1)
                {
                    print("Level 1");
                    lightAttackDamage *= 1.5f;
                    heavyAttackDamage *= 1.5f;
                }
                if (sporeLevel == 2)
                {
                    print("Level 2");
                    lightAttackDamage *= 1.5f;
                    heavyAttackDamage *= 1.5f;
                    speed += 2;
                }
                if (sporeLevel >= 3)
                {
                    print("Level 3+");
                    lightAttackDamage *= 2f;
                    heavyAttackDamage *= 2f;
                    speed += 2;
                }

                effectApplied = true;

                StartCoroutine(RemoveEffect());
            }
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

        StartCoroutine(PlayJumpEffect());
    }

    protected IEnumerator GroundPoundJump()
    {
        moveDir.y = jumpPower;

        isJumping = true;

        float elasped = 0f;
        float totalJumpTime = 0.6f;
        float totalUpTime = 0.2f;
        float totalStallTime = 0.4f;

        while(elasped < totalUpTime)
        {
            elasped += Time.deltaTime;
            moveDir.y = Mathf.Lerp(jumpPower, 0.1f, elasped / totalUpTime);

            controller.Move(moveDir * speed * Time.deltaTime);

            yield return null;
        }

        elasped = 0;

        while (elasped < totalStallTime)
        {
            elasped += Time.deltaTime;
            moveDir.y = Mathf.Lerp(0.1f, 0, elasped / totalStallTime);

            controller.Move(moveDir * speed * Time.deltaTime);

            yield return null;
        }

        elasped = 0;

        while (elasped < totalJumpTime)
        {
            elasped += Time.deltaTime;
            moveDir.y = Mathf.Lerp(-1, -gravity * 5, elasped / totalJumpTime);

            controller.Move(moveDir * speed * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator PlayJumpEffect()
    {
        while(!controller.isGrounded)
        {
            yield return null;
        }

        ParticleManager.Instance.SpawnParticle(ParticleTypes.JumpLanding, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z));
    }

    IEnumerator RemoveEffect()
    {
        yield return new WaitForSeconds(effectSpan);

        effectApplied = false;
    }

    protected virtual void Attack()
    {

    }

    protected bool CanLightAttack()
    {
        if (timeToNextLightAttack < Time.realtimeSinceStartup)
        {
            timeToNextLightAttack = Time.realtimeSinceStartup + lightAttackDelay;
            return true;
        }

        return false;
    }

    protected bool CanHeavyAttack()
    {
        if (timeToNextHeavyAttack < Time.realtimeSinceStartup)
        {
            timeToNextHeavyAttack = Time.realtimeSinceStartup + heavyAttackDelay;
            return true;
        }

        return false;
    }

    void Die()
    {
        isAlive = false;

        StartCoroutine(DeathTimer());

        AnimController.Instance.PlayPlayerDeathAnim(GetComponentInChildren<Animator>(), false);

        dead.Play();
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(deathTimer);

        AnimController.Instance.SetPlayerRespawn(GetComponentInChildren<Animator>(), false);

        GetComponent<Health>().ResetHealth();

        transform.position = respawnLocation.position;

        isAlive = true;
    }

    void CheckRaycastSelection()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;

            rayCastSelection = selection;

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

    protected void AbilityCooldown(bool isAbilityActivated)
    {
        if (isAbilityActivated && isCooldown == false)
        {
            isCooldown = true;
            abilityImage.fillAmount = 1;
        }

        if (isCooldown)
        {
            abilityImage.fillAmount -= 1 / heavyAttackDelay * Time.deltaTime;

            if (abilityImage.fillAmount <= 0)
            {
                isCooldown = false;
            }
        }
    }

    public void CameraLock(bool isLocked)
    {
        if (isLocked)
        {
            thirdPersonCam.enabled = false;
        }
        else
        {
            thirdPersonCam.enabled = true;
        }
    }

    public GameObject[] GetTowers()
    {
        return towers;
    }

    public Effects GetCurrentEffect()
    {
        return currentEffect;
    }

    public void SetEffect(Effects effect)
    {
        currentEffect = effect;
    }

    public void SetEffectApplied(bool applied)
    {
        effectApplied = applied;
    }

    public void SetSporeLevel(int level)
    {
        sporeLevel = level;
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }

    public float GetLightAttackDamage()
    {
        return lightAttackDamage;
    }

    public string GetName()
    {
        return characterName;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public bool GetIsPlayerCharacter()
    {
        return isPlayerCharacter;
    }

    public void SetIsPlayerCharacter(bool isPlayer)
    {
        isPlayerCharacter = isPlayer;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, lightAttackRange);
        Gizmos.DrawWireSphere(attackPoint.position, heavyAttackRange);
    }
}
