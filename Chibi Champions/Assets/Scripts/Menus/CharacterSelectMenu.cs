using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectMenu : MonoBehaviour
{
    [SerializeField]
    GameObject[] characters;

    [SerializeField]
    Text[] UIText;

    private int characterIndex;

    //bool hasSelectedCharacter = false;

    public CharacterDatabase characterDB;
    public Text characterName;
    public Text classType;

    public Button backButton, lockInButton;

    public void ChangeCharacter(int index)
    {
        for(int i = 0; i < characters.Length; ++i)
        {
            characters[i].SetActive(false);
        }
    
        for(int i = 0; i < UIText.Length; ++i)
        {
            UIText[i].text = "";
        }
   
        characterIndex = index;
    
        characters[index].SetActive(true);
    
        Character character = characterDB.GetCharacter(index);
        characterName.text = character.characterName;
        classType.text = character.classType;
    
    //    hasSelectedCharacter = true;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.U))
    //    {
    //        hasSelectedCharacter = true;
    //    }
    //
    //    if (hasSelectedCharacter)
    //    {
    //        lockInButton.interactable = true;
    //    }
    //}

    public void OnClick_Back()
    {
        MenuManager.OpenMenu(Menu.MODE_SELECT, gameObject);
        backButton.transform.localScale = new Vector3(1, 1, 1);

        for (int i = 0; i < characters.Length; ++i)
        {
            characters[i].SetActive(false);
        }
        
        for (int i = 0; i < UIText.Length; ++i)
        {
            UIText[i].text = "";
        }
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