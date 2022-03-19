using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    public int abilityIndex;

    public CharacterSelect databaseSelect;

    Character character;
    CharacterDatabase DB;

    private string header;
    private string content;
    public string control;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Cursor.visible = false;

        DB = databaseSelect.GetCharacterDB();
        character = DB.GetCharacter(databaseSelect.GetCharacterIndex());

        header = character.abilityNames[abilityIndex];
        content = character.abilityDescriptions[abilityIndex];

        AbilityTooltipSystem.Show(content, header, control);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Cursor.visible = true;

        AbilityTooltipSystem.Hide();
    }
}
