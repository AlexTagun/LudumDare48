using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    
    private enum ItemType {
        Torch,
        Shield,
        Sword
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
            case ItemType.Sword:
                _item = new Sword();
                break;
        }
        
        _item.Spawn(transform);
    }

    private void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Hero")) return;
        _item.Collect();
        
        EventManager.HandleOnItemCollect(_item);
        Destroy(gameObject);
    }
}