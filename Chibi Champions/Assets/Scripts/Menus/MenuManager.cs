using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuManager
{
    public static bool isInitialized { get; private set; }
    public static GameObject mainMenu, optionsMenu;

   public static void Init()
    {
        GameObject canvas = GameObject.Find("Canvas");
        mainMenu = canvas.transform.Find("MainMenu").gameObject;
        optionsMenu = canvas.transform.Find("OptionsMenu").gameObject;

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
        }

        callingMenu.SetActive(false);
        
    }
}