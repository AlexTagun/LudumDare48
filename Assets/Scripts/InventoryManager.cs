using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour {
    [SerializeField] private ItemContainer itemContainerPrefab;
    [SerializeField] private Transform inventoryContainer;
    [SerializeField] private Hero heroPrefab;
    private List<ItemContainer> _itemContainers;
    
    private List<Hero> _heroes;

    private static InventoryManager _instance;
    public static InventoryManager Instance => _instance;

    private void Start()
    {
        _instance = this;
        
        EventManager.OnItemSwapped += UpdateItemContainers;
        EventManager.OnItemCollect += OnItemCollect;
        EventManager.OnHpEnded += OnHpEnded;
        EventManager.OnAddHero += OnAddHero;

        _heroes = new List<Hero>();
        for (int i = 0; i < 2; i++) {
            _heroes.Add(HeroFactory.Create(HeroType.Hero));
        }
        
        GameController.CurHeroCount = _heroes.Count;

        _itemContainers = new List<ItemContainer>();

        for (int i = 0; i < _heroes.Count; i++) {
            var container = Instantiate(itemContainerPrefab, inventoryContainer);
            container.gameObject.SetActive(true);
            _itemContainers.Add(container);
        }

        // _itemContainers[0].SetItem(ItemFactory.Create(ItemType.Shield));
        // _itemContainers[1].SetItem(ItemFactory.Create(ItemType.Sword));
        // _itemContainers[2].SetItem(ItemFactory.Create(ItemType.Torch));
        UpdateItemContainers();
    }

    private void UpdateItemContainers() {
        var orderedList = _itemContainers.OrderBy(container => container.transform.GetSiblingIndex()).ToArray();

        for (int i = 0; i < orderedList.Length; i++) {
            orderedList[i].SetData(_heroes[i].CurActivePoints, _heroes[i].MaxActionPoints);
            _heroes[i].SetItem(orderedList[i].Item);
            orderedList[i].SetHero(_heroes[i]);
            orderedList[i].SetColor(_heroes[i].Health.GetHpPercentage());
        }
    }

    private void OnItemCollect(IItem item) {
        var orderedList = _itemContainers.OrderBy(container => container.transform.GetSiblingIndex());

        int i = 0;
        foreach (var itemContainer in orderedList) {
            ++i;
            if (itemContainer.Item != null) continue;
            itemContainer.SetItem(item);
            itemContainer.SetHero(_heroes[i]);
            break;
        }

        UpdateItemContainers();
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
        
        var heroList = _heroes.Where(h => h != null && h.Health.GetHpPercentage() > 0).ToList();

        if (heroList.Count == 0)
        {
            return false;
        }

        var index = Random.Range(0, heroList.Count());

        targetHero = heroList[index];

        return true;
    }

    private void OnHpEnded(Hero hero) {
        var orderedList = _itemContainers.OrderBy(container => container.transform.GetSiblingIndex()).ToArray();

        for (int i = 0; i < _heroes.Count; i++) {
            if (hero != _heroes[i]) continue;
            
            Destroy(orderedList[i].gameObject);
            _itemContainers.Remove(orderedList[i]);
            _heroes[i].Kill();
            _heroes.RemoveAt(i);
            GameController.CurHeroCount--;
        }
    }

    public Transform GetFirstHero() {
        return _heroes.First().transform;
    }

    private void OnDestroy() {
        EventManager.OnItemSwapped -= UpdateItemContainers;
        EventManager.OnItemCollect -= OnItemCollect;
        EventManager.OnHpEnded -= OnHpEnded;
        EventManager.OnAddHero -= OnAddHero;
    }

    private void OnAddHero(Hero hero) {
        _heroes.Add(hero);
        GameController.CurHeroCount = _heroes.Count;
        var container = Instantiate(itemContainerPrefab, inventoryContainer);
        container.gameObject.SetActive(true);
        _itemContainers.Add(container);
        
        UpdateItemContainers();
    }
}