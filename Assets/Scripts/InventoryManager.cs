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

        _heroes = new List<Hero>(FindObjectsOfType<Hero>().OrderBy(hero => hero.name));

        var torch = new Torch();
        var shield = new Shield();
        
        _itemContainers = new List<ItemContainer>();

        for (int i = 0; i < 4; i++) {
            var container = Instantiate(itemContainerPrefab, inventoryContainer);
            container.gameObject.SetActive(true);
            _itemContainers.Add(container);
        }
        
        _itemContainers[0].SetItem(torch);
        _itemContainers[1].SetItem(shield);
        OnItemSwapped();
    }

    private void OnItemSwapped() {
        var orderedList = _itemContainers.OrderBy(container => container.transform.GetSiblingIndex()).ToArray();
        
        for (int i = 0; i < orderedList.Length; i++) {
            if(orderedList[i].Item != null) _heroes[i].SetItem(orderedList[i].Item);
        }
    }
}