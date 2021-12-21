using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float spawnDelay = 3;
    [SerializeField] int maximumSpawns = 10;

    int currentSpawnAmount = 0;
    float timeToNextSpawn = 0;

    private void Update()
    {
        if (CanSpawn())
        {
            SpawnEnemy();
        }
    }

    bool CanSpawn()
    {
        if (currentSpawnAmount >= maximumSpawns)
        {
            return false;
        }

        if (timeToNextSpawn < Time.realtimeSinceStartup)
        {
            timeToNextSpawn = Time.realtimeSinceStartup + spawnDelay;
            return true;
        }

        return false;
    }

    void SpawnEnemy()
    {
        var enemy = EnemyPool.Instance.GetEnemyFromPool();

        enemy.transform.position = transform.position;

        currentSpawnAmount++;
    }
}
