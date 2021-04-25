using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopActivationManager : MonoBehaviour
{
    private void Start() {
        startWaitingShopEnter();
        initShopInteraction();
        // enterTheShop();
    }

    private void initShopInteraction() {
        _shopUI.onProductSelected = processProductSelection;
        _shopUI.onHidden = onExitFromShop;
    }

    private void FixedUpdate() {
        updateShopEnterWaiting();
    }

    private void updateShopEnterWaiting() {
        if (_isWaitingShopEnter) {
            _timeLeftToShopEnter -= Time.fixedDeltaTime;

            if (_timeLeftToShopEnter <= 0f) {
                stopWaitingShopEnter();
                enterTheShop();
            } 
        }
    }

    private void enterTheShop() {
        _shop.rerollProducts(_numItemsToRollInShop);
        _shopUI.show(_shop.products, _wallet);
    }

    private void processProductSelection(Shop.Product inProduct) {
        inProduct.buy(_wallet);
        EventManager.HandleOnItemCollect(ItemFactory.Create(inProduct.ItemType));
    }

    private void onExitFromShop() {
        // startWaitingShopEnter();
    }

    private void stopWaitingShopEnter() {
        _isWaitingShopEnter = false;
    }

    private void startWaitingShopEnter() {
        _isWaitingShopEnter = true;
        _timeLeftToShopEnter = _secondBetweenShopEnter;
    }

    [SerializeField] private float _secondBetweenShopEnter = 30;
    private bool _isWaitingShopEnter = false;
    private float _timeLeftToShopEnter = 0f;

    [SerializeField] private int _numItemsToRollInShop = 6;

    [SerializeField] private Shop _shop = null;
    [SerializeField] private ShopUIObject _shopUI = null;
    [SerializeField] private Wallet _wallet = null;
}
