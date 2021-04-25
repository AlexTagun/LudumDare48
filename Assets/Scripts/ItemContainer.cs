using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour {
    [SerializeField] private Image icon;
    [SerializeField] private Image back;
    [SerializeField] private Image frame;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Color fullHealthColor;
    [SerializeField] private Color halfHealthColor;
    [SerializeField] private Color lowHealthColor;

    public IItem Item;

    public void SetData(int curActivePoints, int maxActivePoints) {
        pointsText.text = $"{curActivePoints}/{maxActivePoints}";
        nameText.text = Item == null ? "" : Item.NameText;
        if(Item == null) icon.sprite = Resources.Load<Sprite>("icon_NoItem");
    }

    public void SetItem(IItem item) {
        Item = item;
        icon.sprite = item.Icon;
    }

    public void SetColor(float percentage) {
        if (percentage >= 0.7f) {
            back.color = fullHealthColor;
            frame.color = fullHealthColor;
            return;
        }

        if (percentage >= 0.25f) {
            back.color = halfHealthColor;
            frame.color = halfHealthColor;
            return;
        }
        
        back.color = lowHealthColor;
        frame.color = lowHealthColor;
    }
}