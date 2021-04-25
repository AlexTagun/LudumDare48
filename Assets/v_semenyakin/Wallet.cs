using System.Collections.Generic;
using UnityEditor;
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
        int sign = inIsPositivePrice ? -1 : 1;

        foreach (KeyValuePair<CurrencyTypes.ECurrencyType, int> currencyPrice in inPrice.currencyPrices)
            if (!isPossibleToChange(currencyPrice.Key, sign * currencyPrice.Value))
                return false;

        return true;
    }

    public bool change(CurrencyTypes.ECurrencyType inType, int inDelta) {
        bool isPossible = isPossibleToChange(inType, inDelta);
        
        if (isPossible) {
            _currencies.addOrGet(inType).value += inDelta;

            save();

            onChanged?.Invoke();
        }

        return isPossible;
    }

    public bool change(CurrencyTypes.Price inPrice, bool inPositivePrice = true) {
        bool isPossible = isPossibleToChange(inPrice, inPositivePrice);

        if (isPossible) {
            foreach (KeyValuePair<CurrencyTypes.ECurrencyType, int> currencyPrice in inPrice.currencyPrices)
                change(currencyPrice.Key, -currencyPrice.Value);

            save();

            onChanged?.Invoke();
        }

        return isPossible;
    }

    public System.Action onChanged = null;

    public int getCurrency(CurrencyTypes.ECurrencyType inType) {
        return _currencies.addOrGet(inType).value;
    }

    private void Start() {
        load();
    }
    
    private int getInitialCurrencyAmount(CurrencyTypes.ECurrencyType inCurrencyType) {
        foreach (InitData_Currency inInitialData_Currency in _initialCurrencies)
            if (inInitialData_Currency._currencyType == inCurrencyType)
                return inInitialData_Currency._amount;

        return 0;
    }

    private static string playerPrefsId_Gold = "gold";

    private void load() {
        int initialGoldAmount = getInitialCurrencyAmount(CurrencyTypes.ECurrencyType.Gold);
        int loadedGoldAmount = PlayerPrefs.GetInt(playerPrefsId_Gold, initialGoldAmount);

        load_set(CurrencyTypes.ECurrencyType.Gold, loadedGoldAmount);
    }

    private void load_set(CurrencyTypes.ECurrencyType inCurrencyType, int inCurrencyAmount) {
        _currencies.addOrGet(inCurrencyType).value = inCurrencyAmount;
    }

    private void save() {
        PlayerPrefs.SetInt(playerPrefsId_Gold, getCurrency(CurrencyTypes.ECurrencyType.Gold));
        PlayerPrefs.Save();
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

public static class PlayerPrefsRemover {
    [MenuItem("Save/Remove User")]
    public static void RemoveUser() {
        PlayerPrefs.DeleteAll();
    }
}
