using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour {
    [SerializeField] private Image icon;
    [SerializeField] private Image back;
    [SerializeField] private Image frame;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Color fullHealthColor;
    [SerializeField] private Color halfHealthColor;
    [SerializeField] private Color lowHealthColor;

    public IItem Item;
    private Hero _hero;

    private void Awake() {
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick() {
        if(Item == null) return;
        
        bool needToDestroy = Item.Click(_hero);
        if (needToDestroy) {
            Item.Destroy();
            Item = null;
            EventManager.HandleOnItemSwapped();
        }
    }

    public void SetData(int curActivePoints, int maxActivePoints) {
        pointsText.text = $"{curActivePoints}/{maxActivePoints}";
        nameText.text = Item == null ? "" : Item.NameText;
        if(Item == null) icon.sprite = Resources.Load<Sprite>("icon_NoItem");
    }

    public void SetItem(IItem item) {
        Item = item;
        icon.sprite = item.Icon;
    }

    public void SetHero(Hero hero) {
        _hero = hero;
    }

    public void SetColor(float percentage) {
        Color color;
        if (percentage >= 0.7f) {
            color = new Color(fullHealthColor.r, fullHealthColor.g, fullHealthColor.b, 0.1f);
            back.color = color;
            frame.color = fullHealthColor;
            return;
        }

        if (percentage >= 0.25f) {
            color = new Color(halfHealthColor.r, halfHealthColor.g, halfHealthColor.b, 0.1f);
            back.color = color;
            frame.color = halfHealthColor;
            return;
        }
        
        color = new Color(lowHealthColor.r, lowHealthColor.g, lowHealthColor.b, 0.1f);
        back.color = color;
        frame.color = lowHealthColor;
    }
}