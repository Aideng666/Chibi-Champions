using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuManager
{
    public static bool isInitialized { get; private set; }
    public static GameObject mainMenu, optionsMenu, modeSelectMenu, lobbyMenu, characterSelectMenu;

   public static void Init()
    {
        GameObject canvas = GameObject.Find("Canvas");
        mainMenu = canvas.transform.Find("MainMenu").gameObject;
        optionsMenu = canvas.transform.Find("OptionsMenu").gameObject;
        modeSelectMenu = canvas.transform.Find("ModeSelectMenu").gameObject;
        lobbyMenu = canvas.transform.Find("LobbyMenu").gameObject;
        characterSelectMenu = canvas.transform.Find("CharacterSelectMenu").gameObject;

        isInitialized = true;
    }

    public static void OpenMenu(Menu menu, GameObject callingMenu)
    {
        if (!isInitialized)
        {
            Init();
        }

        switch (menu)
        {
            case Menu.MAIN_MENU:
                mainMenu.SetActive(true);
                break;

            case Menu.OPTIONS:
                optionsMenu.SetActive(true);
                break;

            case Menu.MODE_SELECT:
                modeSelectMenu.SetActive(true);
                break;

            case Menu.LOBBY:
                lobbyMenu.SetActive(true);
                break;

            case Menu.CHARACTER_SELECT:
                characterSelectMenu.SetActive(true);
                break;
        }

        callingMenu.SetActive(false);
        
    }
}