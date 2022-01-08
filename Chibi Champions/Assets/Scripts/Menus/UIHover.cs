using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //bool mouse_over = false;

    public GameObject panel;

    public Text towerName;

    [SerializeField]
    public int towerIndex;

    Character character;
    CharacterDatabase DB;

    public CharacterSelectMenu a;

    public void ShowCharacterInfo()
    {
        DB = a.GetCharacterDB();
        character = DB.GetCharacter(a.GetCharacterIndex());


        //Debug.Log("Character Name: " + character.characterName); 

        towerName.text = character.towerNames[towerIndex];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //mouse_over = true;
        //Debug.Log("Mouse Enter");
        panel.SetActive(true);

        ShowCharacterInfo();
    }   
    
    public void OnPointerExit(PointerEventData eventData)
    {
        //mouse_over = false;
        //Debug.Log("Mouse Exit");
        panel.SetActive(false);
    }
}