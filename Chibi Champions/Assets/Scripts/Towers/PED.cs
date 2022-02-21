using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PED : Tower
{
    [SerializeField] GameObject sporePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (CanAttack())
        {
            Attack();
        }
    }

    protected override void Attack(GameObject enemy = null)
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();

        foreach (PlayerController player in players)
        {
            var spore = Instantiate(sporePrefab, firePoint.position, Quaternion.identity);
            
            spore.GetComponent<Spore>().SetTower(this);
            spore.GetComponent<Spore>().SetPlayer(player);
        }
    }
}
