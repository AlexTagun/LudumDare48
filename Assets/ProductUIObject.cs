﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductUIObject : MonoBehaviour
{
    public void init(Shop.Product inProduct) {
        _product = inProduct;

        _itemIcon.sprite = inProduct.itemIcon;

        int goldPrice = inProduct.price.getCurrencyPrice(CurrencyTypes.ECurrencyType.Gold);
        _goldAmountText.text = goldPrice.ToString();
    }

    [SerializeField] private UnityEngine.UI.Image _itemIcon = null;
    [SerializeField] private UnityEngine.UI.Text _goldAmountText = null;
    
    private Shop.Product _product = null;
}
