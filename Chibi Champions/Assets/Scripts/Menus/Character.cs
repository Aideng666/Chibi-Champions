using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character 
{
    public string characterName;
    public string classType;

    public Sprite[] towerSprites;
    public Sprite[] abilitySprites;
    public string[] towerNames;
    public string[] towerDescriptions;
    public string[] abilityNames;
    public string[] abilityDescriptions;
}