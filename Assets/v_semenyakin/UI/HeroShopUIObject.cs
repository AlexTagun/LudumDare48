using System.Collections.Generic;
using UnityEngine;

public class HeroShopUIObject : MonoBehaviour
{
    private void Start() {
        initDescendEvent();
    }

    private void initDescendEvent() {
        _descendButton.onClick.AddListener(hide);
    }

    public void show(List<HeroShop.HeroContract> inContracts, Wallet inWallet) {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        clearContractUIs();

        foreach (HeroShop.HeroContract contract in inContracts)
            addContractUI(contract, inWallet);
    }

    public System.Action<HeroShop.HeroContract> onContractSelected;
    public System.Action onHidden;

    public void hide() {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        onHidden?.Invoke();
    }

    private void clearContractUIs() {
        foreach (Transform contractUITransform in _heroContractsGroup.transform)
            Destroy(contractUITransform.gameObject);
    }

    private void addContractUI(HeroShop.HeroContract inContract, Wallet inWallet) {
        HeroContractUIObject newContractUIObject = Instantiate(_heroContractUIPrefab);
        newContractUIObject.transform.SetParent(_heroContractsGroup.transform, false);

        newContractUIObject.init(inContract, inWallet);
        newContractUIObject.onSelected = onContractSelected;
    }

    [SerializeField] private UnityEngine.UI.GridLayoutGroup _heroContractsGroup;
    [SerializeField] private HeroContractUIObject _heroContractUIPrefab;
    [SerializeField] private UnityEngine.UI.Button _descendButton;
}
