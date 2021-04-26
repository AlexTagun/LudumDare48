using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroContractSetup", menuName = "ScriptableObjects/HeroContractSetup", order = 1)]
public class HeroContractSetup : ScriptableObject
{
    public HeroType _heroType;
    public List<CurrencyTypes.Price.InitData_CurrencyPrice> _price;
}
