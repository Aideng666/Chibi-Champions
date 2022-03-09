using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] Canvas interactMenu;

    bool towerMenuOpen;

    public static CanvasManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        ApplyCursorLock();
    }

    private void Start()
    {
        interactMenu.gameObject.SetActive(false);
        if (!FindObjectOfType<AudioManager>().IsPlaying("Level") && !FindObjectOfType<AudioManager>().IsPlaying("Level"))
        {
            FindObjectOfType<AudioManager>().Play("Level");
        }
    }

    public void OpenTowerMenu()
    {
        interactMenu.gameObject.SetActive(true);
        RemoveCursorLock();
        towerMenuOpen = true;
    }

    public void CloseTowerMenu()
    {
        interactMenu.gameObject.SetActive(false);
        ApplyCursorLock();
        towerMenuOpen = false;
    }

    void ApplyCursorLock()
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
}
