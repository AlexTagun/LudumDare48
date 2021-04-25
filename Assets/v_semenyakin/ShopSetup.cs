using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopSetup", menuName = "ScriptableObjects/ShopSetup", order = 1)]
public class ShopSetup : ScriptableObject
{
    [System.Serializable]
    public struct ShopSetup_ProductGroupSetup
    {
        [System.Serializable]
        public struct ShopSetup_ProductGroupSetup_ProductSetup
        {
            public int _weight;
            public ShopProductSetup _product;
        };

        public int _weight;
        public List<ShopSetup_ProductGroupSetup_ProductSetup> _products;
    };

    public List<ShopSetup_ProductGroupSetup> _groups;
}