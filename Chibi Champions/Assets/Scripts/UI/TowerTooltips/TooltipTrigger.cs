using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Checks whether to call the show or hide functions if the mouse
// is hovered over the object

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    public int towerIndex;

    public CharacterSelect databaseSelect;

    Character character;
    CharacterDatabase DB;

    private string header;
    private string content;
    private string cost;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Cursor.visible = false;

        DB = databaseSelect.GetCharacterDB();
        character = DB.GetCharacter(databaseSelect.GetCharacterIndex());

        header = character.towerNames[towerIndex];
        content = character.towerDescriptions[towerIndex];
        cost = character.towerBaseCosts[towerIndex].ToString();

        TooltipSystem.Show(content, header, cost);
        Debug.Log("HOVERING OVER TOWER ICON");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Cursor.visible = true;

        TooltipSystem.Hide();
    }
}
