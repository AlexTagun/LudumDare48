using UnityEngine;

public class SecretShowActivationManager : MonoBehaviour
{
    [SerializeField] private Shop _shop = null;
    [SerializeField] private int _itemsInShop = 3;
    [SerializeField] private Collider _interactionCollider = null;

    private Wallet _cache_wallet;
    private ShopUIObject _cache_shopUI;

    private void Start() {
        initCache();
        initShopInteraction();
    }

    private void initCache() {
        _cache_wallet = FindObjectOfType<Wallet>();
        _cache_shopUI = initCache_findShopUI();
    }

    ShopUIObject initCache_findShopUI() {
        //NB: We should to find Shop UI in such way because FindObjectOfType ignores not active objects
        foreach (ShopUIObject ui in Resources.FindObjectsOfTypeAll(typeof(ShopUIObject)) as ShopUIObject[]) {
            if (!UnityEditor.EditorUtility.IsPersistent(ui.transform.root.gameObject) &&
                !(ui.hideFlags == HideFlags.NotEditable || ui.hideFlags == HideFlags.HideAndDontSave))
            {
                return ui;
            }
        }

        return null;
    }

    private void initShopInteraction() {
        shopUI.onProductSelected = processProductSelection;
        shopUI.onHidden = onExitFromShop;
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Hero")) return;

        Collect();
    }

    private void Collect() {
        _interactionCollider.enabled = false;
        enterTheShop();
    }

    private void enterTheShop() {
        _shop.rerollProducts(_itemsInShop);
        shopUI.show(_shop.products, wallet);
    }

    private void onExitFromShop() {
        Destroy(gameObject);
    }

    private void processProductSelection(Shop.Product inProduct) {
        inProduct.buy(wallet);
        EventManager.HandleOnItemCollect(ItemFactory.Create(inProduct.ItemType));
    }

    private Wallet wallet => _cache_wallet;
    private ShopUIObject shopUI => _cache_shopUI;
}
