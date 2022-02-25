using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] PlayerController[] characterList = new PlayerController[3];

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
    }
}
