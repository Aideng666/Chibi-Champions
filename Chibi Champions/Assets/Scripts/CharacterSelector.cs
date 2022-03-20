using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] PlayerController[] characterList = new PlayerController[3];

    public CharacterDatabase characterDB;
    public Image characterPortrait;

    Character character;

    PlayerController activeCharacter;
    // Start is called before the first frame update
    void Start()
    {
        activeCharacter = characterList[PlayerPrefs.GetInt("CharacterIndex")];

        for (int i = 0; i < characterList.Length; i++)
        {
            if (PlayerPrefs.GetInt("CharacterIndex") == i)
            {
                characterList[i].gameObject.SetActive(true);
                activeCharacter = characterList[i];
            }
        }

        character = characterDB.GetCharacter(PlayerPrefs.GetInt("CharacterIndex"));
        characterPortrait.sprite = character.characterPortrait;
    }
}
