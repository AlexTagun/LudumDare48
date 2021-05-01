using System;

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

    public static Action<Hero> OnCurHeroCountUpdated;

    public static void HandleOnCurHeroCountUpdated(Hero hero)
    {
        OnCurHeroCountUpdated?.Invoke(hero);
    }

    public static Action<int> OnLevelChanged;

    public static void HandleOnLevelChanged(int level)
    {
        OnLevelChanged?.Invoke(level);
    }
}