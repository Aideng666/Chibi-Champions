using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton, optionsButton;

    public void OnClick_Play()
    {
        MenuManager.OpenMenu(Menu.LOBBY, gameObject);
        playButton.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnClick_Options()
    {
        MenuManager.OpenMenu(Menu.OPTIONS, gameObject);
        optionsButton.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnClick_Quit()
    {
        Application.Quit();
    }
}