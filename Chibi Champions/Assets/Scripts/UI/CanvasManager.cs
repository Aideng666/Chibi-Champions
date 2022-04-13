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
    [SerializeField] GameObject deathPanel;
    [SerializeField] TMP_Text respawnTimerText;

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

        if (!AudioManager.Instance.IsPlaying("Level") && !AudioManager.Instance.IsPlaying("Level"))
        {
            AudioManager.Instance.Play("Level");
            AudioManager.Instance.Loop("Level");
        }    
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
        //AudioListener.pause = false;
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
        //AudioListener.pause = true;
    }

    public void MultiplayerPause()
    {
        RemoveCursorLock();
        pausePanel.SetActive(true);
        isMultiplayerPaused = true;
    }

    public void ShowDeathPanel(float respawnTime)
    {
        deathPanel.SetActive(true);
        respawnTimerText.text = respawnTime.ToString("0");
    }

    public void HideDeathPanel()
    {
        deathPanel.SetActive(false);
    }
}
