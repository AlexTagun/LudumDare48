using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProductUIObject : MonoBehaviour
{
    public void init(Shop.Product inProduct, Wallet inWallet) {
        _productToReturnOnSelect = inProduct;

        _itemIcon.sprite = inProduct.itemIcon;

        int goldPrice = inProduct.price.getCurrencyPrice(CurrencyTypes.ECurrencyType.Gold);
        _goldAmountText.text = goldPrice.ToString();

        _selectButton.onClick.AddListener(()=>{
            onSelected?.Invoke(_productToReturnOnSelect);
        }); 

        _walletToCheckSelectionPossibility = inWallet;
        inWallet.onChanged = updateSelectionPossibility;
        
        updateSelectionPossibility();
    }

    private void updateSelectionPossibility() {
        _selectButton.interactable = _walletToCheckSelectionPossibility.isPossibleToChange(
            _productToReturnOnSelect.price);
    }

    public System.Action<Shop.Product> onSelected;

    [SerializeField] private UnityEngine.UI.Image _itemIcon = null;
    [SerializeField] private TextMeshProUGUI _goldAmountText = null;
    [SerializeField] private UnityEngine.UI.Button _selectButton = null;

    private Wallet _walletToCheckSelectionPossibility = null;
    private Shop.Product _productToReturnOnSelect = null;
}
