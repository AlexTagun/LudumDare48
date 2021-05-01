
public static class SpeedUpHeroManager
{
    public static void Init()
    {
        EventManager.OnLevelChanged += TrySpeedUpHeroes;
    }

    private static void TrySpeedUpHeroes(int currentLevel)
    {
        if (!InventoryManager.Instance.IsHaveHero())
        {
            return;
        }

        if (!TryGetSpeed(currentLevel, out var speed))
        {
            return;
        }

        InventoryManager.Instance.SetSpeedAllHeroes(speed);
    }

    private static bool TryGetSpeed(int currentLevel, out int speed)
    {
        speed = 0;
        
        if (5 <= currentLevel && currentLevel < 10)
        {
            speed = 6;
            return true;
        }
        
        if (10 <= currentLevel && currentLevel < 15)
        {
            speed = 7;
            return true;
        }

        if (15 <= currentLevel && currentLevel < 20)
        {
            speed = 8;
            return true;
        }
        
        if (20 <= currentLevel && currentLevel < 25)
        {
            speed = 9;
            return true;
        }

        if (25 <= currentLevel)
        {
            speed = 10;
            return true;
        }

        return false;
    }
    
    public static void Clear()
    {
        EventManager.OnLevelChanged -= TrySpeedUpHeroes;
    }
}
