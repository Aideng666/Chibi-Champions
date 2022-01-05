using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectMenu : MonoBehaviour
{

    public Button backButton, singlePlayerButton, multiPlayerButton;

    public void OnClick_Back()
    {
        MenuManager.OpenMenu(Menu.MAIN_MENU, gameObject);
        backButton.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnClick_SinglePlayer()
    {
        MenuManager.OpenMenu(Menu.CHARACTER_SELECT, gameObject);
        singlePlayerButton.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnClick_MultiPlayer() 
    {
        MenuManager.OpenMenu(Menu.LOBBY, gameObject);
        multiPlayerButton.transform.localScale = new Vector3(1, 1, 1);
    }
}