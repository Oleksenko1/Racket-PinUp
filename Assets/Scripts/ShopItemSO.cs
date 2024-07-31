using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "ScriptableObjects/New shop item", order = 1)]
public class ShopItemSO : ScriptableObject
{
    public enum ItemType
    { 
        RacketAccuracy,
        RacketSize,
        RacketSpeed,
        Background,
        Music
    }
    public Sprite shopIcon;
    public ItemType itemType;
    public string title;
    public string description;
    public string levelSaveName;
    [Tooltip("Cost to upgrade from level X to level X+1")]
    public int[] levelsCost;
    [Tooltip("Element 0 stores basic stat of racket with out upgrades")]
    public float[] levelUpgrades;
}
