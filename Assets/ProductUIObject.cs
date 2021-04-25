using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductUIObject : MonoBehaviour
{
    public void init(Shop.Product inProduct) {
        _productToReturnOnSelect = inProduct;

        _itemIcon.sprite = inProduct.itemIcon;

        int goldPrice = inProduct.price.getCurrencyPrice(CurrencyTypes.ECurrencyType.Gold);
        _goldAmountText.text = goldPrice.ToString();

        _selectButton.onClick.AddListener(()=>{
            onSelected?.Invoke(_productToReturnOnSelect);
        });
    }

    public System.Action<Shop.Product> onSelected;

    [SerializeField] private UnityEngine.UI.Image _itemIcon = null;
    [SerializeField] private UnityEngine.UI.Text _goldAmountText = null;
    [SerializeField] private UnityEngine.UI.Button _selectButton = null;

    private Shop.Product _productToReturnOnSelect = null;
}
