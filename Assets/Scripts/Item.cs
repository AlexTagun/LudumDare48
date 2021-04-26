using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem {
    Sprite Icon { get; }
    string NameText { get; }
    void Equip(Transform transform);

    GameObject Spawn(Vector3 position);

    void Collect();

    void Destroy();
    
    bool Click(Hero hero);
}

public enum ItemType {
    None,
    Torch,
    Shield,
    Sword,
    HpPotion,
    ActionPotion
}

public static class ItemFactory {
    public static IItem Create(ItemType type) {
        switch (type) {
            case ItemType.None: return null;
            case ItemType.Torch: return new Torch();
            case ItemType.Shield: return new Shield();
            case ItemType.Sword: return new Sword();
            case ItemType.HpPotion: return new HpPotion();
            case ItemType.ActionPotion: return new ActionPotion();
        }

        return null;
    }
}

public abstract class Item : IItem {
    protected GameObject _prefab;
    protected GameObject _spawnPrefab;
    protected abstract string Name { get; }
    protected abstract string EquipPath { get; }

    public Sprite Icon { get; }
    public virtual string NameText { get; }

    private GameObject _itemGO;
    private GameObject _spawnItemGO;
    
    public Item() {
        Icon = Resources.Load<Sprite>(Name);
        _prefab = Resources.Load<GameObject>(Name);
        _spawnPrefab = Resources.Load<GameObject>($"{Name}_spawn");
    }

    public void Equip(Transform transform) {
        if (_itemGO != null) GameObject.Destroy(_itemGO);

        var wrist = transform.Find(EquipPath);
        _itemGO = GameObject.Instantiate(_prefab, wrist);
    }

    public GameObject Spawn(Vector3 position) {
        if (_spawnItemGO != null) GameObject.Destroy(_spawnItemGO);

        _spawnItemGO = GameObject.Instantiate(_spawnPrefab);
        _spawnItemGO.transform.position = position;
        
        var spawnObject = _spawnItemGO.GetComponent<SpawnObject>();
        spawnObject.SetItem(this);
        return _spawnItemGO;
    }

    public void Collect() {
        if (_spawnItemGO != null) GameObject.Destroy(_spawnItemGO);
    }

    public void Destroy() {
        if (_itemGO != null) GameObject.Destroy(_itemGO);
        if (_spawnItemGO != null) GameObject.Destroy(_spawnItemGO);
    }

    public virtual bool Click(Hero hero) {
        return false;
    }
}

public class Shield : Item {
    protected override string Name => "shield";
    public override string NameText => "Shield";

    protected override string EquipPath =>
        "NPC_walk/Root_M/Pelvis_M/PelvisPart1_M/PelvisPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M";
}

public class Torch : Item {
    protected override string Name => "torch";
    public override string NameText => "Torch";

    protected override string EquipPath =>
        "NPC_walk/Root_M/Pelvis_M/PelvisPart1_M/PelvisPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/ElbowPart1_L/ElbowPart2_L/Wrist_L";
}

public class Sword : Item {
    protected override string Name => "sword";
    public override string NameText => "Sword";

    protected override string EquipPath =>
        "NPC_walk/Root_M/Pelvis_M/PelvisPart1_M/PelvisPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/ElbowPart1_L/ElbowPart2_L/Wrist_L";
}

public class HpPotion : Item {
    protected override string Name => "hpPotion";
    public override string NameText => "Hp Potion";

    protected override string EquipPath =>
        "NPC_walk/Root_M/Pelvis_M/PelvisPart1_M/PelvisPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/ElbowPart1_R/ElbowPart2_R/Wrist_R";
    
    public override bool Click(Hero hero) {
        hero.Health.Heal(50);
        return true;
    }
}

public class ActionPotion : Item {
    protected override string Name => "actionPotion";
    public override string NameText => "Action Potion";

    protected override string EquipPath =>
        "NPC_walk/Root_M/Pelvis_M/PelvisPart1_M/PelvisPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/ElbowPart1_R/ElbowPart2_R/Wrist_R";

    public override bool Click(Hero hero) {
        hero.AddActivePoints(1);
        return true;
    }
}