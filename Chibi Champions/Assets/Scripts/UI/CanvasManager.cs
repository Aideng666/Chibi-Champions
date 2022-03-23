using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] Canvas interactMenu;
    [SerializeField] TMP_Text interactText;

    bool towerMenuOpen;

    //[SerializeField] Animator panelAnim;

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

            FindObjectOfType<AudioManager>().Loop("Level");

        }
    }

    public void OpenTowerMenu()
    {
        interactText.alpha = 0;
        interactMenu.gameObject.SetActive(true);
        RemoveCursorLock();
        towerMenuOpen = true;
        //ShowPanel();
    }

    public void CloseTowerMenu()
    {
        interactText.alpha = 1;
        interactMenu.gameObject.SetActive(false);
        ApplyCursorLock();
        towerMenuOpen = false;
        //HidePanel();
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

    //public void ShowPanel()
    //{
    //    panelAnim.SetTrigger("pressed");
    //}

    //public void HidePanel()
    //{
    //    panelAnim.SetTrigger("pressed");
    //}
}
