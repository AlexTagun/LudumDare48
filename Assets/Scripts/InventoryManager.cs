using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour {
    [SerializeField] private ItemContainer itemContainerPrefab;
    [SerializeField] private Transform inventoryContainer;
    private List<ItemContainer> _itemContainers;
    
    private List<Hero> _heroes;

    private static InventoryManager _instance;
    public static InventoryManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
        
        EventManager.OnItemSwapped += OnItemSwapped;
        EventManager.OnItemCollect += OnItemCollect;

        _heroes = new List<Hero>(FindObjectsOfType<Hero>().OrderBy(hero => hero.name));

        var torch = new Torch();
        var shield = new Shield();
        var sword = new Sword();

        _itemContainers = new List<ItemContainer>();

        for (int i = 0; i < 4; i++) {
            var container = Instantiate(itemContainerPrefab, inventoryContainer);
            container.gameObject.SetActive(true);
            _itemContainers.Add(container);
        }

        _itemContainers[0].SetItem(torch);
        _itemContainers[1].SetItem(shield);
        _itemContainers[2].SetItem(sword);
        OnItemSwapped();
    }

    private void OnItemSwapped() {
        var orderedList = _itemContainers.OrderBy(container => container.transform.GetSiblingIndex()).ToArray();

        for (int i = 0; i < orderedList.Length; i++) {
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

        OnItemSwapped();
    }

    public bool TryGetNearHero(out Hero targetHero, Vector3 fromPosition, float minSqrMagnitude)
    {
        targetHero = null;

        foreach (var hero in _heroes)
        {
            if (hero == null)
            {
                continue;
            }

            var targetSqrMagnitude = (fromPosition - hero.transform.position).sqrMagnitude;

            if (minSqrMagnitude > targetSqrMagnitude)
            {
                minSqrMagnitude = targetSqrMagnitude;
                targetHero = hero;
            }
        }

        return targetHero != null;
    }

    public bool TryGetRandomHero(out Hero targetHero)
    {
        targetHero = null;
        
        var heroList = _heroes.Where(h => h != null).ToList();

        if (heroList.Count == 0)
        {
            return false;
        }

        var index = Random.Range(0, heroList.Count());

        targetHero = heroList[index];

        return true;
    }
}