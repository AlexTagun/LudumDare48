using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

    private IItem _item;

    public void SetItem(IItem item) {
        _item = item;
    }
}