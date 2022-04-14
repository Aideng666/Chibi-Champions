using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potter : PlayerController
{
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] GameObject paintballPrefab;
    [SerializeField] GameObject inkBlastPrefab;
    [SerializeField] GameObject aimObject;
    [SerializeField] float shotSpeed;
    [SerializeField] float healAmount;

    [SerializeField] AudioSource shot;
    [SerializeField] AudioSource blast;

    bool InkBlastActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        shot.volume = AudioManager.Instance.GetSFXVolume();
        blast.volume = AudioManager.Instance.GetSFXVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioManager.Instance.dirtyPot)
        {
            if (AudioManager.Instance.isMute() == true)
            {
                shot.mute = true;
                blast.mute = true;
            }
            else
            {
                shot.mute = false;
                blast.mute = false;
            }

            shot.volume = AudioManager.Instance.GetSFXVolume();
            blast.volume = AudioManager.Instance.GetSFXVolume();
            AudioManager.Instance.dirtyPot = false;

        }


        if (isPlayerCharacter)
        {
            base.Update();

            AbilityCooldown(InkBlastActivated);
        }
    }

    protected override void Attack()
    {
        if (!CanvasManager.isGamePaused && !CanvasManager.isMultiplayerPaused)
        {
            if (Input.GetMouseButton(0) && CanLightAttack())
            {
                if (FindObjectOfType<UDPClient>() != null)
                {
                    UDPClient.Instance.SendPlayerUpdates(ActionTypes.Attack, GetCharacterNameEnum());
                }

                AnimController.Instance.PlayPlayerAttackAnim(GetComponentInChildren<Animator>());

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                Vector3 direction = new Vector3();

                if (Physics.Raycast(ray, out hit, 1000f, ~interactableLayer))
                {
                    var endPoint = hit.point;

                    if (endPoint != null)
                    {
                        direction = (endPoint - attackPoint.position).normalized;
                    }
                }
                shot.Play();

                direction = (aimObject.transform.position - attackPoint.position).normalized;

                var paintball = ProjectilePool.Instance.GetPaintballFromPool(attackPoint.position);

                paintball.GetComponentInChildren<Rigidbody>().velocity = direction * shotSpeed;

            }
            if (Input.GetMouseButton(1) && CanHeavyAttack())
            {
                if (FindObjectOfType<UDPClient>() != null)
                {
                    UDPClient.Instance.SendPlayerUpdates(ActionTypes.Ability, GetCharacterNameEnum());
                }

                AnimController.Instance.PlayPlayerAbilityAnim(GetComponentInChildren<Animator>());

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                Vector3 direction = new Vector3();

                InkBlastActivated = true;

                if (Physics.Raycast(ray, out hit, 1000f, ~interactableLayer))
                {
                    var endPoint = hit.point;

                    if (endPoint != null)
                    {
                        direction = (endPoint - attackPoint.position).normalized;
                    }
                }
                blast.Play();

                direction = (aimObject.transform.position - attackPoint.position).normalized;

                var inkBlast = Instantiate(inkBlastPrefab, attackPoint.position, Quaternion.identity);

                inkBlast.GetComponentInChildren<Rigidbody>().velocity = direction * shotSpeed;

                Destroy(inkBlast, 3);
            }
            else
            {
                InkBlastActivated = false;
            }
        }    
    }

    public override void ReceiveAttackTrigger()
    {
        AnimController.Instance.PlayPlayerAttackAnim(GetComponentInChildren<Animator>());

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        Vector3 direction = new Vector3();

        if (Physics.Raycast(ray, out hit, 1000f, ~interactableLayer))
        {
            var endPoint = hit.point;

            if (endPoint != null)
            {
                direction = (endPoint - attackPoint.position).normalized;
            }
        }

        var paintball = ProjectilePool.Instance.GetPaintballFromPool(attackPoint.position);

        paintball.GetComponentInChildren<Rigidbody>().velocity = direction * shotSpeed;
    }

    public override void ReceiveAbilityTrigger()
    {
        AnimController.Instance.PlayPlayerAbilityAnim(GetComponentInChildren<Animator>());

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        Vector3 direction = new Vector3();

        InkBlastActivated = true;

        if (Physics.Raycast(ray, out hit, 1000f, ~interactableLayer))
        {
            var endPoint = hit.point;

            if (endPoint != null)
            {
                direction = (endPoint - attackPoint.position).normalized;
            }
        }

        var needle = Instantiate(inkBlastPrefab, attackPoint.position, Quaternion.identity);

        needle.GetComponentInChildren<Rigidbody>().velocity = direction * shotSpeed;

        Destroy(needle, 3);
    }

    public float GetHealAmount()
    {
        return healAmount;
    }
}
