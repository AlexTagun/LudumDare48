
public static class SpeedUpHeroManager
{
    private static SpeedUpHeroConfig _speedUpHeroConfig;
    
    public static void Init()
    {
        _speedUpHeroConfig = DataManager.SpeedUpHeroConfig;
        
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
        var datas = _speedUpHeroConfig.SpeedUpHeroDatas;
        
        speed = 0;

        foreach (var speedUpHeroData in datas)
        {
            if (speedUpHeroData.MinLevel <= currentLevel && currentLevel < speedUpHeroData.MaxLevel)
            {
                speed = speedUpHeroData.Speed;
                return true;
            }
        }

        return false;
    }
    
    public static void Clear()
    {
        EventManager.OnLevelChanged -= TrySpeedUpHeroes;
    }
}
