using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [SerializeField] List<EnemySpawner> enemySpawners;
    [SerializeField] GameObject gruntPrefab;
    [SerializeField] GameObject sharpshooterPrefab;

    [SerializeField] TMP_Text currentWaveText;
    [SerializeField] TMP_Text totalWavesText;
    [SerializeField] TMP_Text numberOfEnemiesText;

    int currentWave;
    int numberOfWaves;

    bool waveCompleteAlertFired;
    bool beginWaveAlertFired;

    bool waveCompletePointsAdded = false;

    List<List<GameObject>> enemiesLists = new List<List<GameObject>>();
    List<List<int>> enemyLevels = new List<List<int>>();

    PlayerController[] playerList = new PlayerController[3];

    public static WaveManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentWave = 0;

        InitEnemiesLists();

        playerList = FindObjectsOfType<PlayerController>();

        totalWavesText.text = numberOfWaves.ToString();
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
            spawner.SetLevelList(enemyLevels[currentWave]);
        }
        currentWave++;

        currentWaveText.text = currentWave.ToString();

        numberOfEnemiesText.text = Instance.GetEnemyCount(currentWave).ToString();
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
            enemyLevels.Add(new List<int>());

            string line = reader.ReadLine();

            string[] amountOfEachEnemy = line.Split(':');

            int numberOfLvl1Grunts = Int32.Parse(amountOfEachEnemy[0]);
            int numberOfLvl2Grunts = Int32.Parse(amountOfEachEnemy[1]);
            int numberOfLvl3Grunts = Int32.Parse(amountOfEachEnemy[2]);
            int numberOfLvl1Shooters = Int32.Parse(amountOfEachEnemy[3]);
            int numberOfLvl2Shooters = Int32.Parse(amountOfEachEnemy[4]);
            int numberOfLvl3Shooters = Int32.Parse(amountOfEachEnemy[5]);


            for (int j = 0; j < numberOfLvl1Grunts; j++)
            {
                enemiesLists[i].Add(gruntPrefab);
                enemyLevels[i].Add(1);
            }
            for (int j = 0; j < numberOfLvl2Grunts; j++)
            {
                enemiesLists[i].Add(gruntPrefab);
                enemyLevels[i].Add(2);
            }
            for (int j = 0; j < numberOfLvl3Grunts; j++)
            {
                enemiesLists[i].Add(gruntPrefab);
                enemyLevels[i].Add(3);
            }
            for (int j = 0; j < numberOfLvl1Shooters; j++)
            {
                enemiesLists[i].Add(sharpshooterPrefab);
                enemyLevels[i].Add(1);
            }
            for (int j = 0; j < numberOfLvl2Shooters; j++)
            {
                enemiesLists[i].Add(sharpshooterPrefab);
                enemyLevels[i].Add(2);
            }
            for (int j = 0; j < numberOfLvl3Shooters; j++)
            {
                enemiesLists[i].Add(sharpshooterPrefab);
                enemyLevels[i].Add(3);
            }
        }

        reader.Close();
    }

    public int GetEnemyCount(int waveNum)
    {
        return enemiesLists[waveNum].Count;
    }
}
