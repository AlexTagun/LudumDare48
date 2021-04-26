using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProductUIObject : MonoBehaviour
{
    public void init(Shop.Product inProduct, Wallet inWallet) {
        _product = inProduct;

        _itemIcon.sprite = inProduct.itemIcon;

        int goldPrice = inProduct.price.getCurrencyPrice(CurrencyTypes.ECurrencyType.Gold);
        _goldAmountText.text = goldPrice.ToString();

        _selectButton.onClick.AddListener(processSelection); 

        _walletToCheckSelectionPossibility = inWallet;
        inWallet.onChanged.AddListener(updateSelectionPossibility);
        
        updateSelectionPossibility();
    }

    private void processSelection() {
        onSelected?.Invoke(_product);

        updateSelectionPossibility();
    }


    private static float boughtIconAlpha = 0.1f;
    private void updateSelectionPossibility() {
        switch (_product.getBuyPossibility(_walletToCheckSelectionPossibility))
        {
            case Shop.Product.EBuyPossibility.Possible:
                setIconAlpha(1f);
                _selectButton.interactable = true;
                break;

            case Shop.Product.EBuyPossibility.ImpossibleItemWasBought:
                setIconAlpha(boughtIconAlpha);
                _selectButton.interactable = false;
                break;

            case Shop.Product.EBuyPossibility.ImpossibleNotEnoughCurrency:
                setIconAlpha(1f);
                _selectButton.interactable = false;
                break;
        }
    }
    
    private void setIconAlpha(float inAlpha) {
        Color currentColor = _itemIcon.color;
        currentColor.a = inAlpha;
        _itemIcon.color = currentColor;
    }


    public System.Action<Shop.Product> onSelected;

    [SerializeField] private UnityEngine.UI.Image _itemIcon = null;
    [SerializeField] private TextMeshProUGUI _goldAmountText = null;
    [SerializeField] private UnityEngine.UI.Button _selectButton = null;

    private Wallet _walletToCheckSelectionPossibility = null;
    private Shop.Product _product = null;
}
