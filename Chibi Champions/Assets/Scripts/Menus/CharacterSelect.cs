using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterSelect : MonoBehaviour
{
    [Header("Character UI Models")]
    public GameObject[] characterModels;

    [Header("UI Text")]
    [SerializeField]
    TMP_Text[] towerUIText;
    [SerializeField]
    TMP_Text[] abilityUIText;

    private int characterIndex;

    [Header("Character DB and UI Components")]
    public CharacterDatabase characterDB;
    public TMP_Text characterName;
    public TMP_Text characterClass;
    public Image[] towerSprites;
    public Image[] abilitySprites;

    Character character;

    //Changes the character based on its index
    // 0 for Character1
    // 1 for Character2
    // 2 for Character3
    public void ChangeCharacter(int index)
    {
        // Shows 3D models in the UI
        for (int i = 0; i < characterModels.Length; ++i)
        {
            characterModels[i].SetActive(false);
            characterModels[i].transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        characterModels[index].SetActive(true);

        characterIndex = index;

        //Debug.Log("Character Index: " + characterIndex);

        // Sets the character specific information to the appropriate UI components
        // Grabs the selected character by its index
        character = characterDB.GetCharacter(characterIndex);
        // Sets the name of the character
        characterName.text = character.characterName;
        // Sets the class of the character
        characterClass.text = character.classType;

        // Displays the tower sprites for the selected character
        for (int i = 0; i < towerSprites.Length; ++i)
        {
            towerSprites[i].sprite = character.towerSprites[i];
        }

        // Displays the ability sprites for the selected character
        for (int i = 0; i < abilitySprites.Length; ++i)
        {
            abilitySprites[i].sprite = character.abilitySprites[i];
        }

        // Displays the tower name for each of the towers of the selected character
        for (int i = 0; i < towerUIText.Length; ++i)
        {
            towerUIText[i].text = character.towerNames[i];
        }

        // Displays the ability name for each of the abilities of the selected character
        for (int i = 0; i < abilityUIText.Length; ++i)
        {
            abilityUIText[i].text = character.abilityNames[i];
        }
    }

    public void LockIn()
    {
        PlayerPrefs.SetInt("CharacterIndex", characterIndex);

        if (PlayerClient.Instance.GetClientStarted())
        { 
            PlayerClient.Instance.SetSelectedCharacterIndex(characterIndex);
        }

        CanvasManager.isGamePaused = false;
    }

    public int GetCharacterIndex()
    {
        return characterIndex;
    }

    public CharacterDatabase GetCharacterDB()
    {
        return characterDB;
    }
}
