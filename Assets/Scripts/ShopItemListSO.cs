using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "ScriptableObjects/Shop items list", order = 1)]
public class ShopItemListSO : ScriptableObject
{
    public List<ShopItemSO> list;
}
