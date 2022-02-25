using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelect : MonoBehaviour
{
    //[Header("UI Text")]
    //[SerializeField]
    //TMP_Text[] UIText;
    // Character Name
    // Character Class 
    // Towers Head Title
    // Abilities Head Title
    // Description

    private int characterIndex;

    [Header("Character DB and UI Components")]
    public CharacterDatabase characterDB;
    public TMP_Text characterName;
    public TMP_Text characterClass;
    public Image characterImage;
    public Image[] towerSprites;
    public Image[] abilitySprites;

    [Header("Buttons")]
    public Button lockInButton;
    public Button backButton;

    Character character;

    bool hasSelected = false;

    //Changes the character based on its index
    // 0 for Character1
    // 1 for Character2
    // 2 for Character3
    public void ChangeCharacter(int index)
    {
        characterIndex = index;

        Debug.Log("Character Index: " + characterIndex);

        character = characterDB.GetCharacter(characterIndex);
        characterName.text = character.characterName;
        characterClass.text = character.classType;
        characterImage.sprite = character.characterSprite;

        for (int i = 0; i < towerSprites.Length; ++i)
        {
            towerSprites[i].sprite = character.towerSprites[i];
        }

        for (int i = 0; i < abilitySprites.Length; ++i)
        {
            abilitySprites[i].sprite = character.abilitySprites[i];
        }
    }
}