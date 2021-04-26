using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroShop : MonoBehaviour
{
    public void rerollContracts(int inContractNumToRoll) {
        _contracts.Clear();

        for (int index = 0; index < inContractNumToRoll; ++index)
            _contracts.Add(rollContract());
    }

    public List<HeroContract> contracts => _contracts;

    //NB: Currently product is something that may be bought only once
    public class HeroContract {

        public class Fabric
        {
            public Fabric(HeroContractSetup inContractSetup) {
                _contractSetup = inContractSetup;
            }

            public HeroContract createContract() {
                return new HeroContract(_contractSetup);
            }

            private HeroContractSetup _contractSetup;
        }

        public HeroContract(HeroContractSetup inSetup) {
            _heroType = inSetup._heroType;
            _price = new CurrencyTypes.Price(inSetup._price);

            _heroIcon = HeroFactory.GetHeroIcon(_heroType);

            _isBought = false;
        }

        public Sprite heroIcon => _heroIcon;
        public CurrencyTypes.Price price => _price;
        public HeroType HeroType => _heroType;


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

        public Hero buy(Wallet inWallet) {
            if (EBuyPossibility.Possible == getBuyPossibility(inWallet)) {
                inWallet.change(_price);

                _isBought = true;
                return HeroFactory.Create(_heroType);
            } else {
                return null;
            }
        }

        private HeroType _heroType;
        private CurrencyTypes.Price _price;

        private bool _isBought;

        private Sprite _heroIcon;
    }

    private void Awake() {
        initFromSetup(_setup);
    }

    private void initFromSetup(HeroShopSetup inSetup) {
        WeightedRandom.Option<Group>[] groupsRandomStreamOptions = new WeightedRandom.Option<Group>[_setup._groups.Count];

        int fillingOptionIndex = 0;
        foreach (HeroShopSetup.HeroShopSetup_HeroesGroupSetup inGroupSetupOption in _setup._groups)
            groupsRandomStreamOptions[fillingOptionIndex++] = new WeightedRandom.Option<Group>(
                inGroupSetupOption._weight,
                new Group(inGroupSetupOption));

        _groupsRandomStream = new WeightedRandom.NonRepeatingWeightedRandomStream<Group>(groupsRandomStreamOptions);
    }

    private HeroContract rollContract() {
        return _groupsRandomStream.getNext().getNextContract().createContract();
    }
    
    [SerializeField] private HeroShopSetup _setup = null;
    private WeightedRandom.NonRepeatingWeightedRandomStream<Group> _groupsRandomStream = null;
    private List<HeroContract> _contracts = new List<HeroContract>();

    // -------------------------

    private class Group {

        public Group(HeroShopSetup.HeroShopSetup_HeroesGroupSetup inSetup) {
            WeightedRandom.Option<HeroContract.Fabric>[] randomStreamOptions =
                new WeightedRandom.Option<HeroContract.Fabric>[inSetup._heroes.Count];

            int fillingOptionIndex = 0;
            foreach (HeroShopSetup.HeroShopSetup_HeroesGroupSetup.HeroShopSetup_HeroesGroupSetup_HeroSetup inSetupOption in inSetup._heroes)
                randomStreamOptions[fillingOptionIndex++] = new WeightedRandom.Option<HeroContract.Fabric>(
                    inSetupOption._weight,
                    new HeroContract.Fabric(inSetupOption._heroContract));

            _heroRandomStream = new WeightedRandom.WeightedRandomStream<HeroContract.Fabric>(randomStreamOptions);
        }

        public HeroContract.Fabric getNextContract() {
            return _heroRandomStream.getNext();
        }

        private WeightedRandom.WeightedRandomStream<HeroContract.Fabric> _heroRandomStream;
    }
}
