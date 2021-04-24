using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
    private IItem _item;

    public void SetItem(IItem item) {
        if (_item != null) _item.Destroy();
        _item = item;
        if (_item != null) _item.Instantiate(transform);
    }
}