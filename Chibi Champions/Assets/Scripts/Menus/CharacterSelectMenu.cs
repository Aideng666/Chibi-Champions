using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectMenu : MonoBehaviour
{
    [Header("Character Images")]
    [SerializeField]
    GameObject[] characters;

    [Header("UI Text")]
    [SerializeField]
    Text[] UIText;

    private int characterIndex;

    //bool hasSelectedCharacter = false;

    [Header("Character DB and UI Components")]
    public CharacterDatabase characterDB;
    public Text characterName;
    public Text classType;
    public Image[] artworkSprites;

    [Header("Buttons")]
    public Button backButton;
    public Button lockInButton;

    [Header("UI Objects")]
    public GameObject towerTitle;
    public GameObject abilityTitle;
    public GameObject[] UIImages;

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

        for (int i = 0; i < artworkSprites.Length; ++i)
        {
            artworkSprites[i].sprite = character.towerSprites[i];
        }

        towerTitle.SetActive(true);
        abilityTitle.SetActive(true);

        for (int i = 0; i < UIImages.Length; ++i)
        {
            UIImages[i].SetActive(true);
        }
    
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

        towerTitle.SetActive(false);
        abilityTitle.SetActive(false);

        for (int i = 0; i < UIImages.Length; ++i)
        {
            UIImages[i].SetActive(false);
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