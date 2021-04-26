using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundStartingShopActivationManager : MonoBehaviour
{
    public void performShopDialogs() {
        enterTheShop(); 
    }

    public UnityEngine.Events.UnityEvent onExitFromDialog;

    private void Start() {
        initShopInteraction();

        performShopDialogs();
    }

    private void initShopInteraction() {
        _shopUI.onProductSelected = processProductSelection;
        _shopUI.onHidden = onExitFromShop;

        _heroShopUI.onContractSelected = processContractSelection;
        _heroShopUI.onHidden = onExitFromHeroesShop;
    }

    //1. Heroes shop dialog
    private void enterTheHeroesShop() {
        _heroShop.rerollContracts(_numHeroesToRoll);
        _heroShopUI.show(_heroShop.contracts, _wallet);
    }

    private void processContractSelection(HeroShop.HeroContract inContract) {
        Hero newHero = inContract.buy(_wallet);
        
        //TODO: >>> Put hero to player's deck <<<
    }

    private void onExitFromHeroesShop() {
        enterTheShop();
    }

    //2. Items shop dialog
    private void enterTheShop() {
        _shop.rerollProducts(_numItemsToRoll);
        _shopUI.show(_shop.products, _wallet);
    }

    private void processProductSelection(Shop.Product inProduct) {
        Item newItem = inProduct.buy(_wallet);
        EventManager.HandleOnItemCollect(newItem);
    }

    private void onExitFromShop() {
        onExitFromDialog.Invoke();
    }

    [SerializeField] private int _numItemsToRoll = 6;
    [SerializeField] private int _numHeroesToRoll = 6;

    [SerializeField] private Shop _shop = null;
    [SerializeField] private ShopUIObject _shopUI = null;
    
    [SerializeField] private HeroShop _heroShop = null;
    [SerializeField] private HeroShopUIObject _heroShopUI = null;
    
    [SerializeField] private Wallet _wallet = null;
}
