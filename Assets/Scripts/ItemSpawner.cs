using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    
    private enum ItemType {
        Torch,
        Shield
    }
    
    [SerializeField] private ItemType type;

    private IItem _item;
    private void Awake() {
        switch (type) {
            case ItemType.Torch:
                _item = new Torch();
                break;
            case ItemType.Shield:
                _item = new Shield();
                break;
        }
        
        _item.Spawn(transform);
    }
}