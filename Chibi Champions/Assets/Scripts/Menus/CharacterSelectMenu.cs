using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CharacterSelectMenu : MonoBehaviour
{
    //[SerializeField]
    //GameObject[] characters;

    //[SerializeField]
    //TMP_Text[] UIText;

    //private int characterIndex;

    //bool hasSelectedCharacter = false;

    //public CharacterDatabase characterDB;
    //public TMP_Text characterName;
    //public TMP_Text characterType;

    public Button backButton, lockInButton;

    //public void ChangeCharacter(int index)
    //{
    //    for(int i = 0; i < characters.Length; ++i)
    //    {
    //        characters[i].SetActive(false);
    //    }
    //
    //    for(int i = 0; i < UIText.Length; ++i)
    //    {
    //        UIText[i].text = "";
    //    }
    //
    //    characterIndex = index;
    //
    //    characters[index].SetActive(true);
    //
    //    Character character = characterDB.GetCharacter(index);
    //    characterName.text = character.characterName;
    //    characterType.text = character.characterType;
    //
    //    hasSelectedCharacter = true;
    //}


    public void OnClick_Back()
    {
        MenuManager.OpenMenu(Menu.MODE_SELECT, gameObject);
        backButton.transform.localScale = new Vector3(1, 1, 1);

        //for (int i = 0; i < characters.Length; ++i)
        //{
        //    characters[i].SetActive(false);
        //}
        //
        //for (int i = 0; i < UIText.Length; ++i)
        //{
        //    UIText[i].text = "";
        //}
    }

    //public void OnClickPlay()
    //{
    //    if (hasSelectedCharacter)
    //    {
    //        SceneManager.LoadScene("Sandbox");
    //        PlayerPrefs.SetInt("CharacterIndex", characterIndex);
    //    } 
    //}
}