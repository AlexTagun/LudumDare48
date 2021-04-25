using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour {
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI nameText;

    public IItem Item;

    public void SetData(int curActivePoints, int maxActivePoints) {
        pointsText.text = $"{curActivePoints}/{maxActivePoints}";
        nameText.text = "";
    }

    public void SetItem(IItem item) {
        Item = item;
        icon.sprite = item.Icon;
        nameText.text = item.NameText;
    }
}