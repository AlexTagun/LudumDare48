using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    public Transform ShootPoint => _shootPoint;

    public int MaxActionPoints;
    public int CurActivePoints;
    
    private IItem _item;

    public void SetItem(IItem item) {
        _item = item;
        if (_item != null) _item.Equip(transform);
    }

    public IItem GetItem() {
        return _item;
    }

    public bool CanDoAction() {
        return CurActivePoints > 0;
    }

    public void SpendActionPoint() {
        CurActivePoints--;
        if (CurActivePoints < 0) CurActivePoints = 0;
        EventManager.HandleOnItemSwapped();
    }
}