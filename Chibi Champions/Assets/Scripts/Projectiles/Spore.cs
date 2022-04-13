using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spore : MonoBehaviour
{
    Tower tower;
    NavMeshAgent navMeshAgent;
    PlayerController targetPlayer;

    [SerializeField] AudioSource bub;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();   
        bub.volume = FindObjectOfType<AudioManager>().GetSFXVolume();

    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<AudioManager>().dirtySPR)
        {
            if (FindObjectOfType<AudioManager>().isMute() == true)
            {
                bub.mute = true;
            }
            else
            {
                bub.mute = false;
            }

            bub.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
            FindObjectOfType<AudioManager>().dirtySPR = false;

        }


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
