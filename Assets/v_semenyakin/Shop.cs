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

    //NB: Currently product is something that may be bought only once
    public class Product {

        public class Fabric
        {
            public Fabric(ShopProductSetup inProductSetup) {
                _productSetup = inProductSetup;
            }

            public Product createProduct() {
                return new Product(_productSetup);
            }

            private ShopProductSetup _productSetup;
        }

        public Product(ShopProductSetup inSetup) {
            _itemType = inSetup._itemType;
            _price = new CurrencyTypes.Price(inSetup._price);

            _itemAkaPrefab = (Item)ItemFactory.Create(_itemType);

            _isBought = false;
        }

        public Sprite itemIcon => _itemAkaPrefab.Icon;
        public CurrencyTypes.Price price => _price;
        public ItemType ItemType => _itemType;


        public enum EBuyPossibility
        {
            Possible,
            ImpossibleItemWasBought,
            ImpossibleNotEnoughCurrency
        };

        public EBuyPossibility getBuyPossibility(Wallet inWallet) {
            if (_isBought)
                return EBuyPossibility.ImpossibleItemWasBought;
            if (!inWallet.isPossibleToChange(_price))
                return EBuyPossibility.ImpossibleNotEnoughCurrency;

            return EBuyPossibility.Possible;
        }

        public Item buy(Wallet inWallet) {
            if (EBuyPossibility.Possible == getBuyPossibility(inWallet)) {
                inWallet.change(_price);

                _isBought = true;
                return (Item)ItemFactory.Create(_itemType);
            } else {
                return null;
            }
        }

        private ItemType _itemType;
        private CurrencyTypes.Price _price;

        private bool _isBought;

        private Item _itemAkaPrefab;
    }

    private void Awake() {
        initFromSetup(_setup);
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
        return _groupsRandomStream.getNext().getNextProduct().createProduct();
    }
    
    [SerializeField] private ShopSetup _setup = null;
    private WeightedRandom.NonRepeatingWeightedRandomStream<Group> _groupsRandomStream = null;
    private List<Product> _products = new List<Product>();

    // -------------------------

    private class Group {

        public Group(ShopSetup.ShopSetup_ProductGroupSetup inSetup) {
            WeightedRandom.Option<Product.Fabric>[] randomStreamOptions =
                new WeightedRandom.Option<Product.Fabric>[inSetup._products.Count];

            int fillingOptionIndex = 0;
            foreach (ShopSetup.ShopSetup_ProductGroupSetup.ShopSetup_ProductGroupSetup_ProductSetup inSetupOption in inSetup._products)
                randomStreamOptions[fillingOptionIndex++] = new WeightedRandom.Option<Product.Fabric>(
                    inSetupOption._weight,
                    new Product.Fabric(inSetupOption._product));

            _productsRandomStream = new WeightedRandom.WeightedRandomStream<Product.Fabric>(randomStreamOptions);
        }

        public Product.Fabric getNextProduct() {
            return _productsRandomStream.getNext();
        }

        private WeightedRandom.WeightedRandomStream<Product.Fabric> _productsRandomStream;
    }
}
