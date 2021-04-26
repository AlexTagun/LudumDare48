using TMPro;
using UnityEngine;

public class HeroContractUIObject : MonoBehaviour
{
    public void init(HeroShop.HeroContract inContract, Wallet inWallet) {
        _contract = inContract;

        _heroIcon.sprite = inContract.heroIcon;

        int goldPrice = inContract.price.getCurrencyPrice(CurrencyTypes.ECurrencyType.Gold);
        _goldAmountText.text = goldPrice.ToString();

        _selectButton.onClick.AddListener(processSelection); 

        _walletToCheckSelectionPossibility = inWallet;
        inWallet.onChanged.AddListener(updateSelectionPossibility);
        
        updateSelectionPossibility();
    }

    private void processSelection() {
        onSelected?.Invoke(_contract);

        updateSelectionPossibility();
    }


    private static float boughtIconAlpha = 0.1f;
    private void updateSelectionPossibility() {
        switch (_contract.getBuyPossibility(_walletToCheckSelectionPossibility))
        {
            case HeroShop.HeroContract.EBuyPossibility.Possible:
                setIconAlpha(1f);
                _selectButton.interactable = true;
                break;

            case HeroShop.HeroContract.EBuyPossibility.ImpossibleItemWasBought:
                setIconAlpha(boughtIconAlpha);
                _selectButton.interactable = false;
                break;

            case HeroShop.HeroContract.EBuyPossibility.ImpossibleNotEnoughCurrency:
                setIconAlpha(1f);
                _selectButton.interactable = false;
                break;
        }
    }
    
    private void setIconAlpha(float inAlpha) {
        Color currentColor = _heroIcon.color;
        currentColor.a = inAlpha;
        _heroIcon.color = currentColor;
    }

    public System.Action<HeroShop.HeroContract> onSelected;

    [SerializeField] private UnityEngine.UI.Image _heroIcon = null;
    [SerializeField] private TextMeshProUGUI _goldAmountText = null;
    [SerializeField] private UnityEngine.UI.Button _selectButton = null;

    private Wallet _walletToCheckSelectionPossibility = null;
    private HeroShop.HeroContract _contract = null;
}
