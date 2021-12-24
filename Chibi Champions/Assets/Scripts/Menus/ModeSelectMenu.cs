using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelectMenu : MonoBehaviour
{
    public void OnClick_Back()
    {
        MenuManager.OpenMenu(Menu.MAIN_MENU, gameObject);
    }

    public void OnClick_SinglePlayer()
    {
        MenuManager.OpenMenu(Menu.CHARACTER_SELECT, gameObject);
    }

    public void OnClick_Multiplayer() { }
}
