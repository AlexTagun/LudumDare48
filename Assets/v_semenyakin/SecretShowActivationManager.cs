using UnityEngine;

public static class ObjUtils
{
    public static T findFirstObjectOfTypeAll<T>() where T : Component {
        foreach (T obj in Resources.FindObjectsOfTypeAll(typeof(T)) as T[]) {
            if (!UnityEditor.EditorUtility.IsPersistent(obj.transform.root.gameObject) &&
                !(obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave))
            {
                return obj;
            }
        }

        return null;
    }
}


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

        //NB: We should to find Shop UI in such way because FindObjectOfType ignores not active objects
        _cache_shopUI = ObjUtils.findFirstObjectOfTypeAll<ShopUIObject>();
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
        if (gameObject)
            Destroy(gameObject);
    }

    private void processProductSelection(Shop.Product inProduct) {
        inProduct.buy(wallet);
        EventManager.HandleOnItemCollect(ItemFactory.Create(inProduct.ItemType));
    }

    private Wallet wallet => _cache_wallet;
    private ShopUIObject shopUI => _cache_shopUI;
}
