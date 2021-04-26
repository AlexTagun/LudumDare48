using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroType
{
    Hero
}

public class HeroFactory : MonoBehaviour {
    [SerializeField] private Hero heroPrefab;
    [SerializeField] private Transform center;

    public static HeroFactory Instance;


    private int _heroCount = 0;
    private void Awake() {
        Instance = this;
    }

    public static Hero Create(HeroType inType) {
        var hero = Instantiate(Instance.heroPrefab);
        var spMovement = hero.GetComponent<SpiralMovement>();
        spMovement.Center = Instance.center;
        
        var characterMovement = hero.GetComponent<CharacterMovement>();
        characterMovement.Index = Instance._heroCount;
        characterMovement.SetPosition();
        
        var health = hero.GetComponent<Health>();
        health.Init();
        
        Instance._heroCount++;
        
        return hero;
    }

    public static Sprite GetHeroIcon(HeroType inType) {
        //TODO: Implement this method
        return null;
    }
}