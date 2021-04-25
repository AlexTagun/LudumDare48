using System.Collections.Generic;
using UnityEngine;

public class ShopUIObject : MonoBehaviour
{
    private void Start() {
        initDescendEvent();
    }

    private void initDescendEvent() {
        _descendButton.onClick.AddListener(hide);
    }

    public void show(List<Shop.Product> inProducts, Wallet inWallet) {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        clearProductUIs();

        foreach (Shop.Product product in inProducts)
            addProductUI(product, inWallet);
    }

    public System.Action<Shop.Product> onProductSelected;
    public System.Action onHidden;

    public void hide() {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        onHidden?.Invoke();
    }

    private void clearProductUIs() {
        foreach (Transform productUITransform in _productsGroup.transform)
            Destroy(productUITransform.gameObject);
    }

    private void addProductUI(Shop.Product inProduct, Wallet inWallet) {
        ProductUIObject newProductUIObject = Instantiate(_productUIPrefab);
        newProductUIObject.transform.SetParent(_productsGroup.transform, false);

        newProductUIObject.init(inProduct, inWallet);
        newProductUIObject.onSelected = onProductSelected;
    }

    [SerializeField] private UnityEngine.UI.GridLayoutGroup _productsGroup;
    [SerializeField] private ProductUIObject _productUIPrefab;
    [SerializeField] private UnityEngine.UI.Button _descendButton;
}
