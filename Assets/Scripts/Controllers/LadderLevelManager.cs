using System;

public static class LadderLevelManager
{
    private static int _currentLevel = 0;

    public static int CurrentLevel => _currentLevel;
    
    public static int GetDisplayCurrentLevel()
    {
        return Math.Max(CurrentLevel - 3, 0);
    }

    public static void Clear()
    {
        _currentLevel = 0;
    }

    public static void LevelUp()
    {
        _currentLevel++;
    }
}
