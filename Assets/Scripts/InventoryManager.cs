﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    [SerializeField] private ItemContainer itemContainerPrefab;
    [SerializeField] private Transform inventoryContainer;
    private List<ItemContainer> _itemContainers;
    private List<Hero> _heroes;

    private void Awake() {
        EventManager.OnItemSwapped += UpdateItemContainers;
        EventManager.OnItemCollect += OnItemCollect;

        _heroes = new List<Hero>(FindObjectsOfType<Hero>().OrderBy(hero => hero.name));
        foreach (var hero in _heroes) {
            hero.CurActivePoints = hero.MaxActionPoints;
        }

        _itemContainers = new List<ItemContainer>();

        for (int i = 0; i < 4; i++) {
            var container = Instantiate(itemContainerPrefab, inventoryContainer);
            container.gameObject.SetActive(true);
            _itemContainers.Add(container);
        }

        _itemContainers[0].SetItem(ItemFactory.Create(ItemType.Torch));
        _itemContainers[1].SetItem(ItemFactory.Create(ItemType.Sword));
        _itemContainers[2].SetItem(ItemFactory.Create(ItemType.Shield));
        UpdateItemContainers();
    }

    private void UpdateItemContainers() {
        var orderedList = _itemContainers.OrderBy(container => container.transform.GetSiblingIndex()).ToArray();

        for (int i = 0; i < orderedList.Length; i++) {
            orderedList[i].SetData(_heroes[i].CurActivePoints, _heroes[i].MaxActionPoints);
            if (orderedList[i].Item != null) _heroes[i].SetItem(orderedList[i].Item);
        }
    }

    private void OnItemCollect(IItem item) {
        var orderedList = _itemContainers.OrderBy(container => container.transform.GetSiblingIndex());

        int i = 0;
        foreach (var itemContainer in orderedList) {
            ++i;
            if (itemContainer.Item != null) continue;
            itemContainer.SetItem(item);
            break;
        }

        UpdateItemContainers();
    }
}