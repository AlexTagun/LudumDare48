using System;
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
        EventManager.OnItemSwapped += OnItemSwapped;

        _heroes = new List<Hero>(FindObjectsOfType<Hero>());
        _itemContainers = new List<ItemContainer>();

        for (int i = 0; i < 4; i++) {
            var container = Instantiate(itemContainerPrefab, inventoryContainer);
            container.gameObject.SetActive(true);
            _itemContainers.Add(container);
        }
    }

    private void OnItemSwapped() {
        var orderedList = _itemContainers.OrderBy(container => container.transform.GetSiblingIndex()).ToArray();
        for (int i = 0; i < orderedList.Length; i++) {
            _heroes[0].SetItem(orderedList[i].Item);
        }
    }
}