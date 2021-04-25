using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour {
    private int _count;

    private void Awake() {
        _count = Random.Range(GameController.CurrentLevel * 5, GameController.CurrentLevel * 10);
    }

    private void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Hero")) return;

        Collect();
        
        Destroy(gameObject);
    }

    private void Collect() {
        //Тут сделать добавление монетки в кошелёк _count
    }
}