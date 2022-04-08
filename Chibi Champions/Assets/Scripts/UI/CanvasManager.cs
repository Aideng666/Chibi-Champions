using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] Canvas interactMenu;
    [SerializeField] TMP_Text interactText;

    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject optionsPanel;

    public static bool isGamePaused = false;
    public static bool isMultiplayerPaused = false;

    bool towerMenuOpen;

    public static float savedTime = 0;

    float timeToSpawn = 0;

    public static CanvasManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        ApplyCursorLock();
    }

    private void Start()
    {
        interactMenu.gameObject.SetActive(false);

        //if (!FindObjectOfType<AudioManager>().IsPlaying("Level") && !FindObjectOfType<AudioManager>().IsPlaying("Level"))
        //{
        //    FindObjectOfType<AudioManager>().Play("Level");
        //    FindObjectOfType<AudioManager>().Loop("Level");
        //    FindObjectOfType<AudioManager>().SetMusicVolume();
            
        //}    
    }

    public void OpenTowerMenu()
    {
        interactText.alpha = 0;
        interactMenu.gameObject.SetActive(true);
        RemoveCursorLock();
        towerMenuOpen = true;
    }

    public void CloseTowerMenu()
    {
        interactText.alpha = 1;
        interactMenu.gameObject.SetActive(false);
        ApplyCursorLock();
        towerMenuOpen = false;
    }

    public void ApplyCursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RemoveCursorLock()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public bool IsTowerMenuOpen()
    {
        return towerMenuOpen;
    }

    public void Resume()
    {
        ApplyCursorLock();
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        timeToSpawn = Time.realtimeSinceStartup + (Time.realtimeSinceStartup - savedTime);

        foreach(EnemySpawner spawner in FindObjectsOfType<EnemySpawner>())
        {
            spawner.SetTimeToNextSpawn(timeToSpawn);
        }
    }
    public void MultiplayerResume()
    {
        ApplyCursorLock();
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        isMultiplayerPaused = false;
    }

    public void Pause()
    {
        RemoveCursorLock();
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        savedTime = Time.realtimeSinceStartup;
    }

    public void MultiplayerPause()
    {
        RemoveCursorLock();
        pausePanel.SetActive(true);
        isMultiplayerPaused = true;
    }

}
