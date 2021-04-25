﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour {
    private int _count;
    private Wallet _wallet_cache;

    private void Awake() {
        _count = Random.Range(GameController.CurrentLevel * 5, GameController.CurrentLevel * 10);
    }

    private void Start() {
        initCache();
    }

    private void initCache() {
        _wallet_cache = FindObjectOfType<Wallet>();
    }

    private void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Hero")) return;

        Collect();
        
        Destroy(gameObject);
    }

    private void Collect() {
        _wallet_cache.change(CurrencyTypes.ECurrencyType.Gold, _count);
    }

    private Wallet wallet => _wallet_cache;
}