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
    [SerializeField] protected CharacterNames characterNameEnum;
    [SerializeField] protected Transform cam;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected float speed = 5;
    [SerializeField] protected float lightAttackRange;
    [SerializeField] protected float heavyAttackRange;
    [SerializeField] protected float lightAttackDamage = 10;
    [SerializeField] protected float abilityDamage = 15;
    [SerializeField] protected float gravity = 1.5f;
    [SerializeField] protected float jumpPower = 10;
    [SerializeField] protected float interactDistance = 3;
    [SerializeField] protected float lightAttackDelay = 0.75f;
    [SerializeField] protected float heavyAttackDelay = 0.75f;
    [SerializeField] protected float deathTimer = 5;
    [SerializeField] protected float effectSpan = 10;
    [SerializeField] protected TextMeshProUGUI interactText;
    [SerializeField] protected GameObject cameraLookAt;
    [SerializeField] protected GameObject[] towers = new GameObject[3];
    [SerializeField] protected Transform respawnLocation;

    [SerializeField] AudioSource jump;
    [SerializeField] AudioSource dead;
    [SerializeField] AudioSource hit;
    [SerializeField] AudioSource refresh;

    [SerializeField] protected Image abilityImage;
    [SerializeField] protected Image abilityImageMain;
    protected bool isCooldown = false;
    [SerializeField] protected CharacterDatabase characterDB;
    Character character;

    float respawnTime = 0;

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

    GameObject radiusToDeactivate;

    Vector3 savedPosForMenuClose;

    float savedBasicAttackDamage;
    float savedAbilityDamage;
    float savedSpeed;

    int deathCount = 0;

    float mouseSensitivity = 1;

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

        respawnTime = deathTimer;
        jump.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
        dead.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
        hit.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
        refresh.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (FindObjectOfType<AudioManager>().dirtyChar)
        {
            if (FindObjectOfType<AudioManager>().isMute() == true)
            {
                jump.mute = true;
                dead.mute = true;
                hit.mute = true;
                refresh.mute = true;
            }
            else
            {
                jump.mute = false;
                dead.mute = false;
                hit.mute = false;
                refresh.mute = false;
            }

            jump.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
            dead.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
            hit.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
            refresh.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
            FindObjectOfType<AudioManager>().dirtyChar = false;
        }



        if (isPlayerCharacter)
        {
            thirdPersonCam.LookAt = cameraLookAt.transform;
            thirdPersonCam.Follow = cameraLookAt.transform;

            if (Input.GetKeyDown(KeyCode.Escape) && !CanvasManager.Instance.IsTowerMenuOpen())
            {            
                if (CanvasManager.isGamePaused)
                {
                    CanvasManager.Instance.Resume();
                    CanvasManager.Instance.ApplyCursorLock();
                }
                else
                {
                    CanvasManager.Instance.Pause();
                    CanvasManager.Instance.RemoveCursorLock();
                }
            }   

            if (gameObject.GetComponent<Health>().GetCurrentHealth() <= 0 && isAlive)
            {
                if (FindObjectOfType<UDPClient>() != null)
                {
                    UDPClient.Instance.SendPlayerUpdates(ActionTypes.Death, GetCharacterNameEnum());
                }

                Die();
            }

            if (!isAlive)
            {
                respawnTime -= 1 * Time.deltaTime;
                CanvasManager.Instance.ShowDeathPanel(respawnTime);
            }

            if (isAlive)
            {
                CanvasManager.Instance.HideDeathPanel();

                ApplyEffect();

                Move();

                Attack();

                CheckRaycastSelection();

                if (canInteract && Input.GetKeyDown(KeyCode.E) && !CanvasManager.Instance.IsTowerMenuOpen())
                {
                    TowerMenu.Instance.SetPlayer(this);
                    CanvasManager.Instance.OpenTowerMenu();
                }
                else if (CanvasManager.Instance.IsTowerMenuOpen() 
                    && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape) 
                    || Vector3.Distance(transform.position, savedPosForMenuClose) > 8))
                {
                    CanvasManager.Instance.CloseTowerMenu();
                }

                if (CanvasManager.Instance.IsTowerMenuOpen() || CanvasManager.isGamePaused || CanvasManager.isMultiplayerPaused)
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
            AnimController.Instance.PlayPlayerJumpAnim(GetComponentInChildren<Animator>());

            if (FindObjectOfType<UDPClient>() != null)
            {
                UDPClient.Instance.SendPlayerUpdates(ActionTypes.Jump, GetCharacterNameEnum());
            }

            StartCoroutine(Jump());

            jump.Play();
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            thirdPersonCam.m_Lens.FieldOfView -= 2;

            if (thirdPersonCam.m_Lens.FieldOfView < 45)
            {
                thirdPersonCam.m_Lens.FieldOfView = 45;
            }

        }
        if (Input.mouseScrollDelta.y < 0)
        {
            thirdPersonCam.m_Lens.FieldOfView += 2;

            if (thirdPersonCam.m_Lens.FieldOfView > 60)
            {
                thirdPersonCam.m_Lens.FieldOfView = 60;
            }
        }

        if (isJumping && controller.isGrounded)
        {
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

        if (!CanvasManager.isGamePaused && !CanvasManager.isMultiplayerPaused)
        {
            if (verticalInput > 0)
            {
                //print("Walking Forward");
                AnimController.Instance.SetPlayerWalking(GetComponentInChildren<Animator>(), true, true);
                AnimController.Instance.SetPlayerWalking(GetComponentInChildren<Animator>(), false, false);
            }
            else if (verticalInput < 0)
            {
                AnimController.Instance.SetPlayerWalking(GetComponentInChildren<Animator>(), true, false);
                AnimController.Instance.SetPlayerWalking(GetComponentInChildren<Animator>(), false, true);
            }
            else
            {
                AnimController.Instance.SetPlayerWalking(GetComponentInChildren<Animator>(), false, true);
                AnimController.Instance.SetPlayerWalking(GetComponentInChildren<Animator>(), false, false);
            }

            if (horizontalInput > 0)
            {
                AnimController.Instance.SetPlayerStrafing(GetComponentInChildren<Animator>(), true, 1);
                AnimController.Instance.SetPlayerStrafing(GetComponentInChildren<Animator>(), false, 0);
            }
            else if (horizontalInput < 0)
            {
                AnimController.Instance.SetPlayerStrafing(GetComponentInChildren<Animator>(), true, 0);
                AnimController.Instance.SetPlayerStrafing(GetComponentInChildren<Animator>(), false, 1);
            }
            else
            {
                AnimController.Instance.SetPlayerStrafing(GetComponentInChildren<Animator>(), false, 0);
                AnimController.Instance.SetPlayerStrafing(GetComponentInChildren<Animator>(), false, 1);
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
            print(effectApplied);

            if (!effectApplied)
            {
                print("Applying Effect");

                savedBasicAttackDamage = lightAttackDamage;
                savedAbilityDamage = abilityDamage;
                savedSpeed = speed;

                if (sporeLevel == 1)
                {
                    lightAttackDamage *= 1.5f;
                    abilityDamage *= 1.5f;
                }
                if (sporeLevel == 2)
                {
                    lightAttackDamage *= 1.5f;
                    abilityDamage *= 1.5f;
                    speed += 2;
                }
                if (sporeLevel >= 3)
                {
                    lightAttackDamage *= 2f;
                    abilityDamage *= 2f;
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
        currentEffect = Effects.None;

        lightAttackDamage = savedBasicAttackDamage;
        abilityDamage = savedAbilityDamage;
        speed = savedSpeed;

        print($"Reset Stats: Damage: {lightAttackDamage}");
    }

    protected virtual void Attack()
    {

    }

    public virtual void ReceiveAttackTrigger()
    {

    }

    public virtual void ReceiveAbilityTrigger()
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

    public void Die()
    {
        isAlive = false;

        deathTimer += deathCount;

        deathCount++;

        print($"You've Died {deathCount} Times, you will respawn in {deathTimer} Seconds");

        respawnTime = deathTimer;

        StartCoroutine(DeathTimer());

        AnimController.Instance.PlayPlayerDeathAnim(GetComponentInChildren<Animator>());

        dead.Play();
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(deathTimer);

        AnimController.Instance.SetPlayerRespawn(GetComponentInChildren<Animator>());

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

                    selection.parent.GetComponent<Tower>().SetTowerRadiusActive(true);

                    radiusToDeactivate = selection.parent.gameObject;

                    savedPosForMenuClose = selection.position;
                }
                else
                {
                    TowerMenu.Instance.SetMenuState(MenuState.Buy);
                    TowerMenu.Instance.SetPlatform(selection);

                    savedPosForMenuClose = selection.position;
                }
            }
            else
            {
                canInteract = false;
                interactText.gameObject.SetActive(false);

                try
                {
                    if (!CanvasManager.Instance.IsTowerMenuOpen())
                    {
                        radiusToDeactivate.GetComponent<Tower>().SetTowerRadiusActive(false);
                    }
                }
                catch
                {

                }
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
                refresh.Play();
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

    public float GetAbilityDamage()
    {
        return abilityDamage;
    }

    public string GetName()
    {
        return characterName;
    }

    public CharacterNames GetCharacterNameEnum()
    {
        return characterNameEnum;
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

        if (isPlayer)
        {
            gameObject.AddComponent<AudioListener>();
        }
    }

    public void SetMouseSensitivity(float value)
    {
        mouseSensitivity = value;

        thirdPersonCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 5 * mouseSensitivity;
        thirdPersonCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 5 * mouseSensitivity;
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

    public void hitSound()
    {
        hit.Play();
    }
}
