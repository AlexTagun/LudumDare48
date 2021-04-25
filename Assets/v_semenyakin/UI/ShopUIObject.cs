using System.Collections.Generic;
using UnityEngine;

public class ShopUIObject : MonoBehaviour
{
    public void show(List<Shop.Product> inProducts) {
        clearProductUIs();

        foreach (Shop.Product product in inProducts)
            addProductUI(product);
    }

    public System.Action<Shop.Product> onProductSelected;

    private void clearProductUIs() {
        foreach (Transform productUITransform in _productsGroup.transform)
            Destroy(productUITransform.gameObject);
    }

    private void addProductUI(Shop.Product inProduct) {
        ProductUIObject newProductUIObject = Instantiate(_productUIPrefab);
        newProductUIObject.transform.SetParent(_productsGroup.transform, false);

        newProductUIObject.init(inProduct);
        newProductUIObject.onSelected = onProductSelected;
    }

    [SerializeField] private UnityEngine.UI.GridLayoutGroup _productsGroup;
    [SerializeField] private ProductUIObject _productUIPrefab;
}
