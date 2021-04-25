﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Collider сollider;
    public Transform ShootPoint => _shootPoint;

    [SerializeField] private GameObject _hintProjectile;
    
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

    public void SetHintProjectileActive(bool isActive)
    {
        if (_hintProjectile.activeSelf != isActive)
        {
            _hintProjectile.SetActive(isActive);
        }
    }


    public void Kill() {
        сollider.enabled = false;
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.AddForce(-Camera.main.transform.forward * 10, ForceMode.Impulse);
        rigidbody.AddTorque(new Vector3(200, 200,200), ForceMode.Impulse);

        StartCoroutine(DestroyCoroutine());
    }
    
    private IEnumerator DestroyCoroutine() {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}