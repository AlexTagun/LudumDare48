using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public void rerollProducts(int inProductsNumToRoll) {
        _products.Clear();

        for (int index = 0; index < inProductsNumToRoll; ++index)
            _products.Add(rollProduct());
    }

    public List<Product> products => _products;

    public class Product {
        public Product(ShopProductSetup inSetup) {
            _itemType = inSetup._itemType;
            _price = new CurrencyTypes.Price(inSetup._price);

            _itemAkaPrefab = (Item)ItemFactory.Create(_itemType);
        }

        public Sprite itemIcon => _itemAkaPrefab.Icon;
        public CurrencyTypes.Price price => _price;

        public Item buy(Wallet inWallet) {
            if (inWallet.change(_price)) {
                //TODO: Do item spawn here
                return (Item)ItemFactory.Create(_itemType);
            } else {
                return null;
            }
        }

        private ItemType _itemType;
        private Item _itemAkaPrefab;
        private CurrencyTypes.Price _price;
    }

    private void Start() {
        initFromSetup(_setup);

        rerollProducts(6);
    }

    private void initFromSetup(ShopSetup inSetup) {
        WeightedRandom.Option<Group>[] groupsRandomStreamOptions = new WeightedRandom.Option<Group>[_setup._groups.Count];

        int fillingOptionIndex = 0;
        foreach (ShopSetup.ShopSetup_ProductGroupSetup inGroupSetupOption in _setup._groups)
            groupsRandomStreamOptions[fillingOptionIndex++] = new WeightedRandom.Option<Group>(
                inGroupSetupOption._weight,
                new Group(inGroupSetupOption));

        _groupsRandomStream = new WeightedRandom.NonRepeatingWeightedRandomStream<Group>(groupsRandomStreamOptions);
    }

    private Product rollProduct() {
        return _groupsRandomStream.getNext().getNextProduct();
    }
    
    [SerializeField] private ShopSetup _setup = null;
    private WeightedRandom.NonRepeatingWeightedRandomStream<Group> _groupsRandomStream = null;
    private List<Product> _products = new List<Product>();

    // -------------------------

    private class Group {

        public Group(ShopSetup.ShopSetup_ProductGroupSetup inSetup) {
            WeightedRandom.Option<Product>[] randomStreamOptions = new WeightedRandom.Option<Product>[inSetup._products.Count];

            int fillingOptionIndex = 0;
            foreach (ShopSetup.ShopSetup_ProductGroupSetup.ShopSetup_ProductGroupSetup_ProductSetup inSetupOption in inSetup._products)
                randomStreamOptions[fillingOptionIndex++] = new WeightedRandom.Option<Product>(
                    inSetupOption._weight,
                    new Product(inSetupOption._product));

            _productsRandomStream = new WeightedRandom.WeightedRandomStream<Product>(randomStreamOptions);
        }

        public Product getNextProduct() {
            return _productsRandomStream.getNext();
        }

        private WeightedRandom.WeightedRandomStream<Product> _productsRandomStream;
    }
}
