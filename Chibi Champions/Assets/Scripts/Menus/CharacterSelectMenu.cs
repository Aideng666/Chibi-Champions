using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectMenu : MonoBehaviour
{
    [SerializeField]
    GameObject[] characters;

    public void ChangeCharacter(int index)
    {
        for(int i = 0; i < characters.Length; ++i)
        {
            characters[i].SetActive(false);
        }

        characters[index].SetActive(true);
    }


    public void OnClick_Back()
    {
        MenuManager.OpenMenu(Menu.MODE_SELECT, gameObject);
    }

    public void OnClickPlay()
    {
        SceneManager.LoadScene("Main");
    }
}