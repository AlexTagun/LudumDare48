using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopProductSetup", menuName = "ScriptableObjects/ShopProductSetup", order = 1)]
public class ShopProductSetup : ScriptableObject
{
    public ItemType _itemType;
    public List<CurrencyTypes.Price.InitData_CurrencyPrice> _price;
}
