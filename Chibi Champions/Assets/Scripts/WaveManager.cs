using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] int numberOfWaves;
    [SerializeField] List<EnemySpawner> enemySpawners;

    int currentWave;

    bool waveCompleteAlertFired;

    List<int> enemiesPerWaveList = new List<int>();
    //List<List<GameObject>> enemiesPerWaveList = new List<List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        currentWave = 0;

        InitWaveList();

        BeginWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckWaveComplete())
        {
            print("Press Q for next wave");

            if (Input.GetKeyDown(KeyCode.Q))
            {
                BeginWave();
            }
        }
    }

    void InitWaveList()
    {
        int numberOfEnemies = 1;

        for (int i = 0; i < numberOfWaves; i++)
        {
            enemiesPerWaveList.Add(numberOfEnemies);

            numberOfEnemies++;

        }
    }

    void BeginWave()
    {
        print("The current Wave is " + (currentWave + 1));

        waveCompleteAlertFired = false;

        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.ResetMaximumSpawns(enemiesPerWaveList[currentWave]);
        }
        currentWave++;
    }

    bool CheckWaveComplete()
    {
        var enemyList = FindObjectsOfType<EnemyController>();
        int trueCheckCount = 0;

        foreach (EnemySpawner spawner in enemySpawners)
        {
            if (spawner.GetFirstEnemySpawned() && enemyList.Length < 1)
            {
                trueCheckCount++;
            }
        }
        if (trueCheckCount == enemySpawners.Count)
        {
            if (!waveCompleteAlertFired)
            {
                AlertManager.Instance.DisplayAlert("Wave Complete!");
                waveCompleteAlertFired = true;
            }

            return true;
        }

        return false;
    }
}
