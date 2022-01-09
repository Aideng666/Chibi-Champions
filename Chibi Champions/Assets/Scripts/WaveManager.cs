using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] int numberOfWaves;
    [SerializeField] List<EnemySpawner> enemySpawners;
    [SerializeField] GameObject gruntPrefab;
    [SerializeField] GameObject sharpshooterPrefab;

    int currentWave;

    bool waveCompleteAlertFired;

    List<List<GameObject>> enemiesLists = new List<List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        currentWave = 0;

        InitEnemiesLists();

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

    void BeginWave()
    {
        print("The current Wave is " + (currentWave + 1));

        waveCompleteAlertFired = false;

        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.SetSpawnList(enemiesLists[currentWave]);
        }
        currentWave++;
    }

    bool CheckWaveComplete()
    {
        var enemyList = FindObjectsOfType<GruntController>();
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

    void InitEnemiesLists()
    {
        StreamReader reader = new StreamReader("Assets/WaveData.txt");

        numberOfWaves = Int32.Parse(reader.ReadLine());

        for (int i = 0; i < numberOfWaves; i++)
        {
            enemiesLists.Add(new List<GameObject>());
            string line = reader.ReadLine();

            string[] amountOfEachEnemy = line.Split(':');

            int numberOfGrunts = Int32.Parse(amountOfEachEnemy[0]);
            int numberOfShooters = Int32.Parse(amountOfEachEnemy[1]);

            for (int j = 0; j < numberOfGrunts; j++)
            {
                enemiesLists[i].Add(gruntPrefab);
            }
            for (int j = 0; j < numberOfShooters; j++)
            {
                enemiesLists[i].Add(sharpshooterPrefab);
            }
        }

        reader.Close();
    }
}
