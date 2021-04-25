using UnityEngine;

public class CurrencyGameUIObject : MonoBehaviour
{
    private void FixedUpdate() {
        updateCurrencyAmountsText();
    }

    private void updateCurrencyAmountsText() {
        int actualGoldAmount = _wallet.getCurrency(CurrencyTypes.ECurrencyType.Gold);
        
        if (actualGoldAmount != _showingGoldAmount) {
            _goldAmountText.text = actualGoldAmount.ToString();
            _showingGoldAmount = actualGoldAmount;
        }
    }

    [SerializeField] private Wallet _wallet = null;

    private int _showingGoldAmount = 0;
    [SerializeField] private TMPro.TextMeshProUGUI _goldAmountText = null;
}
