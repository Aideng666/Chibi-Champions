using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spore : MonoBehaviour
{
    Tower tower;
    NavMeshAgent navMeshAgent;
    PlayerController targetPlayer;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.destination = targetPlayer.transform.position;

        if (Vector3.Distance(transform.position, targetPlayer.transform.position) < 2)
        {
            ActivateSpore();
        }

        if (tower.GetLevel() == 4)
        {
            navMeshAgent.speed = 6;
        }
    }

    void ActivateSpore()
    {
        ParticleManager.Instance.SpawnParticle(ParticleTypes.Spore, transform.position);

        targetPlayer.SetEffectApplied(false);
        targetPlayer.SetSporeLevel(tower.GetLevel());
        targetPlayer.SetEffect(Effects.PED);

        Destroy(gameObject);
    }

    public void SetTower(Tower t)
    {
        tower = t;
    }

    public void SetPlayer(PlayerController player)
    {
        targetPlayer = player;
    }
}
