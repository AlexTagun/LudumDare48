using UnityEngine;

public class CurrencyGameUIObject : MonoBehaviour
{
    private void Start() {
        updateCurrencyAmountsText();
        
        _wallet.onChanged.AddListener(updateCurrencyAmountsText);
    }

    private void updateCurrencyAmountsText() {
        int actualGoldAmount = _wallet.getCurrency(CurrencyTypes.ECurrencyType.Gold);
        _goldAmountText.text = actualGoldAmount.ToString();
    }

    [SerializeField] private Wallet _wallet = null;

    [SerializeField] private TMPro.TextMeshProUGUI _goldAmountText = null;
}
