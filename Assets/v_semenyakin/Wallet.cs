using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtension
{
    public static Value addOrGet<Key, Value>(this Dictionary<Key, Value> inThisDictionary, Key inKey)
        where Value : new()
    {
        if (inThisDictionary.ContainsKey(inKey)) {
            return inThisDictionary[inKey];
        } else {
            Value newValue = new Value();
            inThisDictionary.Add(inKey, newValue);
            return newValue;
        }
    }
}

public class Boxed<T>
{
    public T value
    {
        get { return _value; }
        set { _value = value; }
    }

    private T _value;
};

public class Wallet : MonoBehaviour
{
    public bool isPossibleToChange(CurrencyTypes.ECurrencyType inType, int inDelta) {
        return inDelta >= 0 || _currencies.addOrGet(inType).value >= -inDelta;
    }

    public bool isPossibleToChange(CurrencyTypes.Price inPrice, bool inIsPositivePrice = true) {
        int sign = inIsPositivePrice ? 1 : -1;

        foreach (KeyValuePair<CurrencyTypes.ECurrencyType, int> currencyPrice in inPrice.currencyPrices)
            if (!isPossibleToChange(currencyPrice.Key, sign * currencyPrice.Value))
                return false;

        return true;
    }

    public bool change(CurrencyTypes.ECurrencyType inType, int inDelta) {
        bool isPossible = isPossibleToChange(inType, inDelta);
        
        if (isPossible)
            _currencies.addOrGet(inType).value += inDelta;

        return isPossible;
    }

    public bool change(CurrencyTypes.Price inPrice, bool inPositivePrice = true) {
        bool isPossible = isPossibleToChange(inPrice, inPositivePrice);

        if (isPossible)
            foreach (KeyValuePair<CurrencyTypes.ECurrencyType, int> currencyPrice in inPrice.currencyPrices)
                change(currencyPrice.Key, -currencyPrice.Value);

        return isPossible;
    }

    public int getCurrency(CurrencyTypes.ECurrencyType inType) {
        return _currencies.addOrGet(inType).value;
    }

    private void Start() {
        fillInitialCurrencies();
    }
    
    private void fillInitialCurrencies() {
        foreach (InitData_Currency initData_currency in _initialCurrencies)
            _currencies.addOrGet(initData_currency._currencyType).value += initData_currency._amount;
    }

    [System.Serializable]
    public struct InitData_Currency
    {
        public CurrencyTypes.ECurrencyType _currencyType;
        public int _amount;
    }

    [SerializeField] private List<InitData_Currency> _initialCurrencies = null;

    private Dictionary<CurrencyTypes.ECurrencyType, Boxed<int>> _currencies =
        new Dictionary<CurrencyTypes.ECurrencyType, Boxed<int>>();
}
