using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour {
    [SerializeField] private Image icon;

    public IItem Item;

    public void SetItem(IItem item) {
        icon.sprite = item.Icon;
    }
}