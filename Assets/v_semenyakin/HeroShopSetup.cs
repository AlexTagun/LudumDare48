using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroShopSetup", menuName = "ScriptableObjects/HeroShopSetup", order = 1)]
public class HeroShopSetup : ScriptableObject
{
    [System.Serializable]
    public struct HeroShopSetup_HeroesGroupSetup
    {
        [System.Serializable]
        public struct HeroShopSetup_HeroesGroupSetup_HeroSetup
        {
            public int _weight;
            public HeroContractSetup _heroContract;
        };

        public int _weight;
        public List<HeroShopSetup_HeroesGroupSetup_HeroSetup> _heroes;
    };

    public List<HeroShopSetup_HeroesGroupSetup> _groups;
}
