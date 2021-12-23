using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnClick_Play()
    {
        SceneManager.LoadScene("Main");
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