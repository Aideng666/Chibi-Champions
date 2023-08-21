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

    string[] playersCharacters = new string[3];
    // Start is called before the first frame update
    void Start()
    {
        activeCharacter = characterList[PlayerPrefs.GetInt("CharacterIndex")];

        //if (PlayerClient.Instance.GetClientStarted())
        //{
        //    playersCharacters = PlayerClient.Instance.GetPlayersCharacters();

        //    for (int i = 0; i < playersCharacters.Length; i++)
        //    {
        //        if (playersCharacters[i] != null)
        //        {
        //            if (playersCharacters[i] == "Drumstick")
        //            {
        //                characterList[0].gameObject.SetActive(true);
        //            }
        //            if (playersCharacters[i] == "Rolfe")
        //            {
        //                characterList[1].gameObject.SetActive(true);
        //            }
        //            if (playersCharacters[i] == "Potter")
        //            {
        //                characterList[2].gameObject.SetActive(true);
        //            }
        //        }
        //    }

        //    for (int i = 0; i < characterList.Length; i++)
        //    {
        //        if (PlayerClient.Instance.GetSelectedCharacterIndex() == i)
        //        {
        //            characterList[i].SetIsPlayerCharacter(true);
        //        }
        //    }
        //}
        //else
        //{
            for (int i = 0; i < characterList.Length; i++)
            {
                if (PlayerPrefs.GetInt("CharacterIndex") == i)
                {
                    characterList[i].gameObject.SetActive(true);
                    characterList[i].SetIsPlayerCharacter(true);
                }
            }
        //}

        for (int i = 0; i < characterList.Length; i++)
        {
            if (characterList[i].GetIsPlayerCharacter())
            {
                print("Setting Character Icon");
                character = characterDB.GetCharacter(PlayerPrefs.GetInt("CharacterIndex"));
                characterPortrait.sprite = character.characterPortrait;
            }
        }
    }
}
