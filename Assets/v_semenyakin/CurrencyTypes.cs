using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyTypes
{
    public enum ECurrencyType
    {
        Gold
    }

    public struct Price
    {
        [System.Serializable]
        public struct InitData_CurrencyPrice
        {
            public ECurrencyType type;
            public int amount;
        };

        public Price(List<InitData_CurrencyPrice> inInitData) {
            _currencyPrices = new Dictionary<ECurrencyType, int>();
            foreach (InitData_CurrencyPrice initData_currencyPrice in inInitData)
                _currencyPrices.Add(initData_currencyPrice.type, initData_currencyPrice.amount);
        }

        public Dictionary<ECurrencyType, int> currencyPrices => _currencyPrices;

        public int getCurrencyPrice(ECurrencyType inCurrency) {
            int price;
            bool contains = _currencyPrices.TryGetValue(inCurrency, out price);
            return contains ? price : 0;
        }

        private Dictionary<ECurrencyType, int> _currencyPrices;
    };
}
