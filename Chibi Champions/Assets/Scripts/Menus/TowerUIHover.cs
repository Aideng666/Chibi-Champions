using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerUIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel;

    public Text towerName;
    public Image towerImage;
    public Text towerDesc;
    public Text towerCost;

    [SerializeField]
    public int towerIndex;

    Character character;
    CharacterDatabase DB;

    public void ShowCharacterInfo()
    {
        DB = FindObjectOfType<CharacterSelectMenu>().GetCharacterDB();
        character = DB.GetCharacter(FindObjectOfType<CharacterSelectMenu>().GetCharacterIndex());

        towerName.text = character.towerBaseNames[towerIndex];
        towerImage.sprite = character.towerBaseSprites[towerIndex];
        towerDesc.text = character.towerBaseDescriptions[towerIndex];
        towerCost.text = character.towerBaseCosts[towerIndex];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);

        ShowCharacterInfo();
    }   
    
    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
}