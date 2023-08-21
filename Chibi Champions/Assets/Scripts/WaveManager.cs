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

    bool winStarted;

    bool deactivatedExtraSpawners;

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

        currentWaveText.text = currentWave.ToString();
        totalWavesText.text = numberOfWaves.ToString();

        currentPhaseText.text = "Build Phase";

        AudioManager.Instance.Play("Level");
        AudioManager.Instance.Pause("Level");
        AudioManager.Instance.Loop("Level");
        AudioManager.Instance.Play("BuildPhase");
        AudioManager.Instance.Loop("BuildPhase");
        AudioManager.Instance.SetMusicVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if (!deactivatedExtraSpawners)
        {
            int numberOfPlayers = FindObjectsOfType<PlayerController>().Length;

            switch (numberOfPlayers)
            {
                case 1:

                    for (int i = 4; i < 8; i++)
                    {
                        enemySpawners[i].transform.parent.gameObject.SetActive(false);
                    }

                    break;

                case 2:

                    for (int i = 6; i < 8; i++)
                    {
                        enemySpawners[i].transform.parent.gameObject.SetActive(false);
                    }

                    break;
            }

            deactivatedExtraSpawners = true;
        }


        if (currentWave > 0)
        {
            int numberOfPlayers = FindObjectsOfType<PlayerController>().Length;

            switch (numberOfPlayers)
            {
                case 1:

                    for (int i = 4; i < 8; i++)
                    {
                        enemySpawners[i].transform.parent.gameObject.SetActive(false);
                    }

                    currentLivingEnemies = (enemiesLists[currentWave - 1].Count * 4) - enemiesKilled;

                    break;

                case 2:

                    for (int i = 6; i < 8; i++)
                    {
                        enemySpawners[i].transform.parent.gameObject.SetActive(false);
                    }

                    currentLivingEnemies = (enemiesLists[currentWave - 1].Count * 6) - enemiesKilled;

                    break;

                case 3:

                    currentLivingEnemies = (enemiesLists[currentWave - 1].Count * 8) - enemiesKilled;

                    break;
            }
        }

        numberOfEnemiesText.text = currentLivingEnemies.ToString();

        if (CheckWaveComplete())
        {
            if (currentWave == enemiesLists.Count && !winStarted)
            {
                StartCoroutine(WinGame());

                winStarted = true;
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

                PlayerPrefs.SetInt("WavesCompleted", PlayerPrefs.GetInt("WavesCompleted") + 1);
            }
            
            if (!beginWaveAlertFired)
            {
                if (FindObjectOfType<UDPClient>() != null)
                {
                    if (PlayerClient.Instance.GetClientNum() == 0)
                    {
                        AlertManager.Instance.DisplayAlert(new Alert(Color.white, $"Prepare Your Defences! When Ready, Press Q", 1000));
                    }
                    else
                    {
                        AlertManager.Instance.DisplayAlert(new Alert(Color.white, $"Prepare Your Defences! Waiting For Player 1 To Start Wave", 1000));
                    }
                }
                else
                {
                    AlertManager.Instance.DisplayAlert(new Alert(Color.white, $"Prepare Your Defences! When Ready, Press Q", 1000));
                }

                beginWaveAlertFired = true;
            }

            //if (PlayerClient.Instance.GetClientNum() == 0 && Input.GetKeyDown(KeyCode.Q))
            //{
            //    UDPClient.Instance.SendStartWave();

            //    BeginWave();
            //}
            //else if(FindObjectOfType<UDPClient>() == null && Input.GetKeyDown(KeyCode.Q))
            //{
            //    BeginWave();
            //}
        }
    }

    public void BeginWave()
    {
        AudioManager.Instance.UnPause("Level");
        AudioManager.Instance.Pause("BuildPhase");

        currentPhaseText.text = "Combat Phase";
       
        waveCompleteAlertFired = false;
        waveCompletePointsAdded = false;
        ResetEnemiesKilled();

        int numberOfPlayers = FindObjectsOfType<PlayerController>().Length;

        switch (numberOfPlayers)
        {
            case 1:

                for (int i = 4; i < 8; i++)
                {
                    enemySpawners[i].transform.parent.gameObject.SetActive(false);
                }

                for (int i = 0; i < 4; i++)
                {
                    if (currentWave == 0 && i != 2 && i != 3)
                    {
                        AnimController.Instance.PlayOpenDoorAnim(enemySpawners[i].gameObject.transform.parent.GetComponentInChildren<Animator>());
                        enemySpawners[i].gameObject.transform.parent.GetComponent<AudioSource>().Play();
                    }

                    enemySpawners[i].SetSpawnList(enemiesLists[currentWave]);
                    enemySpawners[i].SetLevelList(enemyLevels[currentWave]);
                }


                break;

            case 2:

                for (int i = 6; i < 8; i++)
                {
                    enemySpawners[i].transform.parent.gameObject.SetActive(false);
                }

                for (int i = 0; i < 6; i++)
                {
                    if (currentWave == 0 && i != 2 && i != 3)
                    {
                        AnimController.Instance.PlayOpenDoorAnim(enemySpawners[i].gameObject.transform.parent.GetComponentInChildren<Animator>());
                        enemySpawners[i].gameObject.transform.parent.GetComponent<AudioSource>().Play();
                    }

                    enemySpawners[i].SetSpawnList(enemiesLists[currentWave]);
                    enemySpawners[i].SetLevelList(enemyLevels[currentWave]);
                }

                break;

            case 3:

                for (int i = 0; i < 8; i++)
                {
                    if (currentWave == 0 && i != 2 && i != 3)
                    {
                        AnimController.Instance.PlayOpenDoorAnim(enemySpawners[i].gameObject.transform.parent.GetComponentInChildren<Animator>());
                        enemySpawners[i].gameObject.transform.parent.GetComponent<AudioSource>().Play();
                    }

                    enemySpawners[i].SetSpawnList(enemiesLists[currentWave]);
                    enemySpawners[i].SetLevelList(enemyLevels[currentWave]);
                }

                break;
        }

        currentWave++;

        currentWaveText.text = currentWave.ToString();

        if (FindObjectsOfType<AlertText>().Length > 0)
        {
            AlertManager.Instance.SetAlertPlaying(false);
            Destroy(FindObjectOfType<AlertText>().gameObject);

            print("Destroyed");
        }

        AlertManager.Instance.DisplayAlert(new Alert(Color.white, $"WAVE STARTED! DEFEND THE CURE!", 5));
    }

    bool CheckWaveComplete()
    {
        if (currentWave == 0)
        {
            return true;
        }

        List<GameObject> enemyListPerSpawner = enemiesLists[currentWave - 1];

        int numberOfPlayers = FindObjectsOfType<PlayerController>().Length;

        switch (numberOfPlayers)
        {
            case 1:

                if (enemiesKilled >= enemyListPerSpawner.Count * 4)
                {
                    if (!waveCompleteAlertFired)
                    {
                        if (PlayerClient.Instance.GetClientNum() <= 0)
                        {
                            AlertManager.Instance.DisplayAlert(new Alert(Color.white, "Wave Complete! When Ready, Press Q", 1000));
                        }
                        else
                        {
                            AlertManager.Instance.DisplayAlert(new Alert(Color.white, "Wave Complete! Waiting for Player 1", 1000));
                        }
                        waveCompleteAlertFired = true;
                        AudioManager.Instance.Pause("Level");
                        AudioManager.Instance.UnPause("BuildPhase");
                    }

                    return true;
                }

                break;

            case 2:

                if (enemiesKilled >= enemyListPerSpawner.Count * 6)
                {
                    if (!waveCompleteAlertFired)
                    {
                        if (PlayerClient.Instance.GetClientNum() <= 0)
                        {
                            AlertManager.Instance.DisplayAlert(new Alert(Color.white, "Wave Complete! When Ready, Press Q", 1000));
                        }
                        else
                        {
                            AlertManager.Instance.DisplayAlert(new Alert(Color.white, "Wave Complete! Waiting for Player 1", 1000));
                        }
                        waveCompleteAlertFired = true;

                        AudioManager.Instance.Pause("Level");
                        AudioManager.Instance.UnPause("BuildPhase");
                    }

                    return true;
                }

                break;

            case 3:

                if (enemiesKilled >= enemyListPerSpawner.Count * 8)
                {
                    if (!waveCompleteAlertFired)
                    {
                        if (PlayerClient.Instance.GetClientNum() <= 0)
                        {
                            AlertManager.Instance.DisplayAlert(new Alert(Color.white, "Wave Complete! When Ready, Press Q", 1000));
                        }
                        else
                        {
                            AlertManager.Instance.DisplayAlert(new Alert(Color.white, "Wave Complete! Waiting for Player 1", 1000));
                        }

                        waveCompleteAlertFired = true;

                        AudioManager.Instance.Pause("Level");
                        AudioManager.Instance.UnPause("BuildPhase");
                    }

                    return true;
                }

                break;
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

    public void WriteDataToLeaderboard(string data)
    {
        StreamReader reader = new StreamReader("Assets/Leaderboards.txt");

        List<string> oldLeaderboard = new List<string>();
        
        while(!reader.EndOfStream)
        {
            oldLeaderboard.Add(reader.ReadLine());
        }

        reader.Close();

        StreamWriter writer = new StreamWriter("Assets/Leaderboards.txt");

        for (int i = 0; i < oldLeaderboard.Count; i++)
        {
            writer.WriteLine(oldLeaderboard[i]);
        }

        writer.WriteLine(data);

        writer.Close();
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
        if (FindObjectOfType<UDPClient>() != null)
        {
            UDPClient.Instance.SendLeaderboardStats();
        }

        AlertManager.Instance.DisplayAlert(new Alert(Color.green, "YOU WIN!!", 2));

        yield return new WaitForSeconds(3);

        CanvasManager.Instance.RemoveCursorLock();

        SceneManager.LoadScene("Win");
    }
}
