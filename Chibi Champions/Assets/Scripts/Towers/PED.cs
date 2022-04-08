using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PED : Tower
{
    [SerializeField] GameObject sporePrefab;
    [SerializeField] AudioSource bloop;
    [SerializeField] AudioSource shake;

    void Update()
    {
        if (FindObjectOfType<AudioManager>().isMute() == true)
        {
            bloop.mute = true;
            shake.mute = true;
        }
        else
        {
            bloop.mute = false;
            shake.mute = false;
        }
        bloop.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
        shake.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
        if (CanAttack())
        {
            Attack();
        }
    }

    protected override void Attack(GameObject enemy = null)
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        bloop.Play();
        foreach (PlayerController player in players)
        {
            var spore = Instantiate(sporePrefab, firePoint.position, Quaternion.identity);
            
            spore.GetComponent<Spore>().SetTower(this);
            spore.GetComponent<Spore>().SetPlayer(player);
        }
    }

    public override void Upgrade()
    {
        if (towerLevel > 3)
        {
            print("Tower is Max Level");
            return;
        }

        base.Upgrade();
    }
}
