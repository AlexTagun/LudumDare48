using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
    private IItem _item;

    public void SetItem(IItem item) {
        _item = item;
        if (_item != null) _item.Equip(transform);
    }

    public IItem GetItem() {
        return _item;
    }
}