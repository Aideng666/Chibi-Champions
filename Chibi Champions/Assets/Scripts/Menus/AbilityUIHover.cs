using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityUIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel;

    public Text abilityName;
    public Image abilityImage;
    public Text abilityDesc;
    public Text abilityCost;

    [SerializeField]
    public int abilityIndex;

    Character character;
    CharacterDatabase DB;

    public void ShowCharacterInfo()
    {
        DB = FindObjectOfType<CharacterSelectMenu>().GetCharacterDB();
        character = DB.GetCharacter(FindObjectOfType<CharacterSelectMenu>().GetCharacterIndex());

        abilityName.text = character.abilityNames[abilityIndex];
        abilityImage.sprite = character.abilitySprites[abilityIndex];
        abilityDesc.text = character.abilityDescriptions[abilityIndex];
        abilityCost.text = character.abilityControls[abilityIndex];
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