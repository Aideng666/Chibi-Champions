using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potter : PlayerController
{
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] GameObject paintballPrefab;
    [SerializeField] GameObject inkBlastPrefab;
    [SerializeField] float shotSpeed;
    [SerializeField] float healAmount;

    bool InkBlastActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerCharacter)
        {
            base.Update();

            AbilityCooldown(InkBlastActivated);
        }
    }

    protected override void Attack()
    {
        if (!CanvasManager.isGamePaused)
        {
            if (Input.GetMouseButton(0) && CanLightAttack())
            {
                if (FindObjectOfType<UDPClient>() != null)
                {
                    UDPClient.Instance.SendPlayerUpdates(ActionTypes.Attack, GetCharacterNameEnum());
                }

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

                //var paintball = Instantiate(paintballPrefab, attackPoint.position, Quaternion.identity);
                var paintball = ProjectilePool.Instance.GetPaintballFromPool(attackPoint.position);

                paintball.GetComponentInChildren<Rigidbody>().velocity = direction * shotSpeed;

            }
            if (Input.GetMouseButton(1) && CanHeavyAttack())
            {
                if (FindObjectOfType<UDPClient>() != null)
                {
                    UDPClient.Instance.SendPlayerUpdates(ActionTypes.Ability, GetCharacterNameEnum());
                }

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
