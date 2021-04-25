using System.Collections.Generic;
using UnityEngine;

public class ShopUIObject : MonoBehaviour
{
    private void Start() {
        show(_debug_shop.products);
    }

    public void show(List<Shop.Product> inProducts) {
        foreach (Shop.Product product in inProducts)
            addProductUI(product);
    }

    private void addProductUI(Shop.Product inProduct) {
        ProductUIObject newProductUIObject = Instantiate(_productUIPrefab);
        newProductUIObject.transform.SetParent(_productsGroup.transform, false);

        newProductUIObject.init(inProduct);
    }

    [SerializeField] private UnityEngine.UI.GridLayoutGroup _productsGroup;
    [SerializeField] private ProductUIObject _productUIPrefab;

    [SerializeField] private Shop _debug_shop = null;
}
