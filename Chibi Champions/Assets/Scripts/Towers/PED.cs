using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PED : Tower
{
    [SerializeField] GameObject sporePrefab;

    [SerializeField] AudioSource bloop;
    [SerializeField] AudioSource shake;

    private void Start()
    {
        base.StartTower();

        bloop.volume = AudioManager.Instance.GetSFXVolume();
        shake.volume = AudioManager.Instance.GetSFXVolume();
    }
    void Update()
    {
        if (AudioManager.Instance.dirtySAP)
        {
            if (AudioManager.Instance.isMute() == true)
            {
                bloop.mute = true;
                shake.mute = true;
            }
            else
            {
                bloop.mute = false;
                shake.mute = false;
            }
            bloop.volume = AudioManager.Instance.GetSFXVolume();
            shake.volume = AudioManager.Instance.GetSFXVolume();
            AudioManager.Instance.dirtySAP = false;

        }



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
