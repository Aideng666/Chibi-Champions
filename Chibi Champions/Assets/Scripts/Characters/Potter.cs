using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potter : PlayerController
{
    [SerializeField] GameObject paintballPrefab;
    [SerializeField] GameObject healingNeedlePrefab;
    [SerializeField] float shotSpeed;

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

            if (Physics.Raycast(ray, out hit))
            {
                var endPoint = hit.point;

                if (endPoint != null)
                {
                    print("Found EndPoint");

                    direction = (endPoint - attackPoint.position).normalized;
                }
                else
                {
                    print("No EndPoint");
                    direction = cam.forward;

                    direction.y += 0.1f;

                    direction = direction.normalized;
                }
            }

            var paintball = Instantiate(paintballPrefab, attackPoint.position, Quaternion.identity);

            paintball.GetComponentInChildren<Rigidbody>().velocity = direction * shotSpeed;

            Destroy(paintball, 3);
        }
    }
}
