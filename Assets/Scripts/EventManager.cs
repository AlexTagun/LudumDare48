using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {
    public static Action OnItemSwapped;

    public static bool IsDragging;

    public static void HandleOnItemSwapped() {
        OnItemSwapped?.Invoke();
    }

    public static Action<IItem> OnItemCollect;

    public static void HandleOnItemCollect(IItem item) {
        OnItemCollect?.Invoke(item);
    }
    
    public static Action<Hero> OnHpEnded;
    public static void HandleOnHpEnded(Hero hero) {
        OnHpEnded?.Invoke(hero);
    }

    public static Action<Hero> OnAddHero;

    public static void HandleOnAddHero(Hero hero) {
        OnAddHero?.Invoke(hero);
    }
}