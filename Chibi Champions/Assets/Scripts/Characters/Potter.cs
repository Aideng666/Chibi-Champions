using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potter : PlayerController
{
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] GameObject paintballPrefab;
    [SerializeField] GameObject healingNeedlePrefab;
    [SerializeField] float shotSpeed;
    [SerializeField] float healAmount;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        if (Input.GetMouseButton(0) && CanLightAttack())
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

            //var paintball = Instantiate(paintballPrefab, attackPoint.position, Quaternion.identity);
            var paintball = ProjectilePool.Instance.GetPaintballFromPool(attackPoint.position);

            paintball.GetComponentInChildren<Rigidbody>().velocity = direction * shotSpeed;
            
        }
        if (Input.GetMouseButton(1) && CanHeavyAttack())
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

            var needle = Instantiate(healingNeedlePrefab, attackPoint.position, Quaternion.identity);

            needle.transform.localScale *= 2;

            needle.GetComponentInChildren<Rigidbody>().velocity = direction * shotSpeed;

            Destroy(needle, 3);
        }
    }

    public float GetHealAmount()
    {
        return healAmount;
    }
}
