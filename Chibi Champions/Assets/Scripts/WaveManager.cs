using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [SerializeField] List<EnemySpawner> enemySpawners;
    [SerializeField] GameObject gruntPrefab;
    [SerializeField] GameObject sharpshooterPrefab;

    [SerializeField] TMP_Text currentWaveText;
    [SerializeField] TMP_Text totalWavesText;
    [SerializeField] TMP_Text numberOfEnemiesText;

    [SerializeField] TMP_Text currentPhaseText;

    int currentWave;
    int numberOfWaves;

    bool waveCompleteAlertFired;
    bool beginWaveAlertFired;

    bool waveCompletePointsAdded = false;

    List<List<GameObject>> enemiesLists = new List<List<GameObject>>();
    List<List<int>> enemyLevels = new List<List<int>>();

    int enemiesKilled = 0;
    int currentLivingEnemies;

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

        totalWavesText.text = numberOfWaves.ToString();

        currentPhaseText.text = "Build Phase";
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWave > 0)
        {
            currentLivingEnemies = (enemiesLists[currentWave - 1].Count * enemySpawners.Count) - enemiesKilled;
        }

        numberOfEnemiesText.text = currentLivingEnemies.ToString();

        if (CheckWaveComplete())
        {
            if (currentWave == enemiesLists.Count)
            {
                StartCoroutine(WinGame());
            }

            currentPhaseText.text = "Build Phase";

            if (!waveCompletePointsAdded && currentWave > 0)
            {
                PlayerController[] playerList = FindObjectsOfType<PlayerController>();
          
                if (playerList.Length == 1)
                {
                    playerList[0].GetComponent<PointsManager>().AddPoints(300);
                }
                else if (playerList.Length == 2)
                {
                    playerList[0].GetComponent<PointsManager>().AddPoints(200);
                    playerList[1].GetComponent<PointsManager>().AddPoints(200);
                }
                else if (playerList.Length == 3)
                {
                    playerList[0].GetComponent<PointsManager>().AddPoints(100);
                    playerList[1].GetComponent<PointsManager>().AddPoints(100);
                    playerList[2].GetComponent<PointsManager>().AddPoints(100);
                }

                waveCompletePointsAdded = true;
            }
            
            if (!beginWaveAlertFired)
            {
                if (FindObjectOfType<UDPClient>() != null)
                {
                    if (PlayerClient.Instance.GetClientNum() == 0)
                    {
                        AlertManager.Instance.DisplayAlert(new Alert(Color.red, $"Prepare Your Defences! When Ready, Press Q", 1000));
                    }
                    else
                    {
                        AlertManager.Instance.DisplayAlert(new Alert(Color.red, $"Prepare Your Defences! Waiting For Player 1 To Start Wave", 1000));
                    }
                }
                else
                {
                    AlertManager.Instance.DisplayAlert(new Alert(Color.red, $"Prepare Your Defences! When Ready, Press Q", 1000));
                }

                beginWaveAlertFired = true;
            }

            if (PlayerClient.Instance.GetClientNum() == 0 && Input.GetKeyDown(KeyCode.Q))
            {
                UDPClient.Instance.SendStartWave();

                BeginWave();
            }
            else if(FindObjectOfType<UDPClient>() == null && Input.GetKeyDown(KeyCode.Q))
            {
                BeginWave();
            }
        }
    }

    public void BeginWave()
    {
        currentPhaseText.text = "Combat Phase";
       
        waveCompleteAlertFired = false;
        waveCompletePointsAdded = false;
        ResetEnemiesKilled();

        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.SetSpawnList(enemiesLists[currentWave]);
            spawner.SetLevelList(enemyLevels[currentWave]);
        }
        currentWave++;

        currentWaveText.text = currentWave.ToString();

        if (FindObjectsOfType<AlertText>().Length > 0)
        {
            AlertManager.Instance.SetAlertPlaying(false);
            Destroy(FindObjectOfType<AlertText>().gameObject);
        }

        AlertManager.Instance.DisplayAlert(new Alert(Color.red, $"WAVE STARTED! DEFEND THE CURE!", 5));
    }

    bool CheckWaveComplete()
    {
        if (currentWave == 0)
        {
            return true;
        }

        List<GameObject> enemyListPerSpawner = enemiesLists[currentWave - 1];

        if (enemiesKilled == enemyListPerSpawner.Count * enemySpawners.Count)
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

    public void AddEnemyKilled()
    {
        enemiesKilled++;
    }

    public void ResetEnemiesKilled()
    {
        enemiesKilled = 0;
    }

    public int GetCurrentLivingEnemies()
    {
        return currentLivingEnemies;
    }

    IEnumerator WinGame()
    {
        AlertManager.Instance.DisplayAlert(new Alert(Color.green, "YOU WIN!!", 2));

        yield return new WaitForSeconds(3);

        CanvasManager.Instance.RemoveCursorLock();

        SceneManager.LoadScene("Win");
    }
}
