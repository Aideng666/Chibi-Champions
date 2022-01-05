using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuManager
{
    public static bool isInitialized { get; private set; }
    public static GameObject mainMenu, optionsMenu, lobbyMenu;

   public static void Init()
    {
        GameObject canvas = GameObject.Find("Canvas");
        mainMenu = canvas.transform.Find("MainMenu").gameObject;
        optionsMenu = canvas.transform.Find("OptionsMenu").gameObject;
        lobbyMenu = canvas.transform.Find("LobbyMenu").gameObject;

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

            case Menu.LOBBY:
                lobbyMenu.SetActive(true);
                break;
        }

        callingMenu.SetActive(false);
        
    }
}