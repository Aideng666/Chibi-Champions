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
    public int[] towerBaseCosts;
    // Might need to make array of each cost per tower for each upgrade
    // public int[] tower1UpgradeCosts
    // public int[] tower2UpgradeCosts
    // public int[] tower3UpgradeCosts
    // Also need to make array of the upgrade images for each tower and their desc
    // public Sprite[] tower1UpgradeSprites
    // public Sprite[] tower2UpgradeSprites
    // public Sprite[] tower3UpgradeSprites
    // public string[] tower1UpgradeNames
    // public string[] tower2UpgradeNames
    // public string[] tower3UpgradeNames
    // public string[] tower1UpgradeDescription
    // public string[] tower2UpgradeDescription
    // public string[] tower3UpgradeDescription
}