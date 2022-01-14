using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character 
{
    public string characterName;
    public string classType;

    public Sprite[] towerBaseSprites;
    public Sprite[] abilitySprites;
    public string[] towerBaseNames;
    public string[] towerBaseDescriptions;
    public string[] towerBaseCosts;
    public string[] abilityNames;
    public string[] abilityDescriptions;
    public string[] abilityControls;
}