using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderHouse : Tower
{
    [SerializeField] GameObject spiderPrefab;
    [SerializeField] float spiderSpawnAmount;
    [SerializeField] float effectTickDelay;
    [SerializeField] int maximumSpiders = 6;
    [SerializeField] AudioSource hatch;

    int currentSpiders = 0;

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<AudioManager>().isMute() == true)
        {
            hatch.mute = true;
        }
        else
        {
            hatch.mute = false;
        }
        hatch.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
        //hatch.maxDistance = attackRange;
        //hatch.minDistance = hatch.maxDistance - 2;
        UpdateView();

        if (CanAttack())
        {
            Attack();
        }
    }

    protected override void Attack(GameObject enemy = null)
    {
        for (int i = 0; i < spiderSpawnAmount; i++)
        {
            if (currentSpiders < maximumSpiders)
            {
                var spider = Instantiate(spiderPrefab, new Vector3(firePoint.position.x, 0, firePoint.position.z), Quaternion.identity);

                currentSpiders++;
                spider.GetComponent<Spider>().SetTower(this);
                spider.GetComponent<Spider>().SetTickDelay(effectTickDelay);
                hatch.Play();
            }
        }

    }

    public override void Upgrade()
    {
        if (towerLevel == 1)
        {
            effectTickDelay /= 1.5f;
        }
        else if (towerLevel == 2)
        {
            attackRange += 3f;
        }
        else if (towerLevel == 3)
        {
            spiderSpawnAmount += 2;
        }
        else
        {
            print("Tower is Max Level");
            return;
        }

        base.Upgrade();
    }

    public float GetTickDelay()
    {
        return effectTickDelay;
    }

    public void RemoveSpider()
    {
        currentSpiders--;
    }
}
