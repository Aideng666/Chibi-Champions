using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackBeacon : MonoBehaviour
{
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] int maxPulses = 3;
    [SerializeField] float pulseDelay = 2;
    [SerializeField] float pulseRange = 10;

    int currentPulses = 0;

    float timeBeforeNextPulse;

    // Start is called before the first frame update
    void Start()
    {
        timeBeforeNextPulse = Time.time + pulseDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPulses >= maxPulses)
        {
            Destroy(gameObject);
            FindObjectOfType<Rolfe>().RemoveBeacon();
        }

        if (ShouldPulse())
        {
            Pulse();
        }
    }

    bool ShouldPulse()
    {
        if (timeBeforeNextPulse < Time.time)
        {
            timeBeforeNextPulse = Time.time + pulseDelay;
            return true;
        }

        return false;
    }

    void Pulse()
    {
        ParticleManager.Instance.SpawnParticle(ParticleTypes.Knockback, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z));

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, pulseRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                enemy.GetComponentInParent<Enemy>().Knockback(50, FindObjectOfType<Cure>().transform);
            }
        }

        currentPulses++;
    }
}
