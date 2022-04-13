using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float spawnDelay = 3;

    int currentSpawnAmount = 0;
    float timeToNextSpawn = 0;

    bool firstEnemySpawned;

    List<GameObject> spawnList = new List<GameObject>();
    List<int> levelList = new List<int>();

    private void Update()
    {
        if (!CanvasManager.isGamePaused)
        {
            if (CanSpawn())
            {
                SpawnEnemy();
            }                               
        }
    }

    bool CanSpawn()
    {
        if (currentSpawnAmount >= spawnList.Count)
        {
            return false;
        }

        if (timeToNextSpawn < Time.time)
        {
            timeToNextSpawn = Time.time + spawnDelay;
            return true;
        }

        return false;
    }

    void SpawnEnemy()
    {
        if (spawnList[currentSpawnAmount].name.Contains("Grunt"))
        {
            var enemy = EnemyPool.Instance.GetGruntFromPool(transform.position);

            enemy.GetComponent<GruntController>().SetLevel(levelList[currentSpawnAmount]);

            enemy.transform.position = transform.position;
        }
        else if(spawnList[currentSpawnAmount].name.Contains("SharpShooter"))
        {
            var enemy = EnemyPool.Instance.GetShooterFromPool(transform.position);

            enemy.GetComponent<SharpshooterController>().SetLevel(levelList[currentSpawnAmount]);

            enemy.transform.position = transform.position;
        }

        currentSpawnAmount++;

        firstEnemySpawned = true;
    }

    public bool GetFirstEnemySpawned()
    {
        return firstEnemySpawned;
    }

    public void SetSpawnList(List<GameObject> list)
    {
        spawnList = list;
        currentSpawnAmount = 0;
        firstEnemySpawned = false;
    }

    public void SetLevelList(List<int> list)
    {
        levelList = list;
    }

    public void SetTimeToNextSpawn(float timeToSpawn)
    {
        timeToNextSpawn = timeToSpawn;
    }
}
