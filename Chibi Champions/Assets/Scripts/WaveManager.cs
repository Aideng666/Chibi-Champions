using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] List<EnemySpawner> enemySpawners;
    [SerializeField] GameObject gruntPrefab;
    [SerializeField] GameObject sharpshooterPrefab;

    int currentWave;
    int numberOfWaves;

    bool waveCompleteAlertFired;
    bool beginWaveAlertFired;

    bool waveCompletePointsAdded = false;

    List<List<GameObject>> enemiesLists = new List<List<GameObject>>();

   PlayerController[] playerList = new PlayerController[3];

    // Start is called before the first frame update
    void Start()
    {
        currentWave = 0;

        InitEnemiesLists();

        playerList = FindObjectsOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckWaveComplete())
        {
            if (!waveCompletePointsAdded)
            {
                foreach (PlayerController player in playerList)
                {
                    if (player != null)
                    {
                        player.GetComponent<PointsManager>().AddPoints(200);
                    }
                }

                waveCompletePointsAdded = true;
            }
            
            if (FindObjectsOfType<AlertText>().Length == 0 && !beginWaveAlertFired)
            {
                AlertManager.Instance.DisplayAlert(new Alert(Color.red, $"Press Q To Begin Wave {currentWave + 1}", 3));
                beginWaveAlertFired = true;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                BeginWave();

                if (FindObjectsOfType<AlertText>().Length > 0)
                {
                    Destroy(FindObjectOfType<AlertText>().gameObject);
                }
            }
        }
    }

    void BeginWave()
    {
        print("The current Wave is " + (currentWave + 1));

        waveCompleteAlertFired = false;
        waveCompletePointsAdded = false;

        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.SetSpawnList(enemiesLists[currentWave]);
        }
        currentWave++;
    }

    bool CheckWaveComplete()
    {
        if (currentWave == 0)
        {
            return true;
        }

        var enemyList = FindObjectsOfType<Enemy>();
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
                AlertManager.Instance.DisplayAlert(new Alert(Color.red, "Wave Complete!"));
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
