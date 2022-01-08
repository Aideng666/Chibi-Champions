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

    [Header("Character DB and UI Components")]
    public CharacterDatabase characterDB;
    public Text characterName;
    public Text classType;
    public Image[] artworkSprites;
    public Image[] abilityArtworkSprites;

    [Header("Buttons")]
    public Button backButton;
    public Button lockInButton;

    [Header("UI Objects")]
    public GameObject towerTitle;
    public GameObject abilityTitle;
    public GameObject[] UIImages;

    Character character;

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

        character = characterDB.GetCharacter(index);
        character = characterDB.GetCharacter(characterIndex);
        characterName.text = character.characterName;
        classType.text = character.classType;

        for (int i = 0; i < artworkSprites.Length; ++i)
        {
            artworkSprites[i].sprite = character.towerSprites[i];
        }

        for (int i = 0; i < abilityArtworkSprites.Length; ++i)
        {
            abilityArtworkSprites[i].sprite = character.abilitySprites[i];
        }

        towerTitle.SetActive(true);
        abilityTitle.SetActive(true);

        for (int i = 0; i < UIImages.Length; ++i)
        {
            UIImages[i].SetActive(true);
        }
    }

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

    [SerializeField]
    public int GetCharacterIndex()
    {
        return characterIndex;
    }

    [SerializeField]
    public CharacterDatabase GetCharacterDB()
    {
        return characterDB;
    }

    [SerializeField]
    public Character GetCharacter()
    {
        return character;
    }
}