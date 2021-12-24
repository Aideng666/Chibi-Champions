using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnClick_Play()
    {
        MenuManager.OpenMenu(Menu.MODE_SELECT, gameObject);
    }

    public void OnClick_Options()
    {
        MenuManager.OpenMenu(Menu.OPTIONS, gameObject);
    }

    public void OnClick_Quit()
    {
        Application.Quit();
    }
}