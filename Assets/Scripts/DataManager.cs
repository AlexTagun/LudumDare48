using UnityEngine;

public static class DataManager
{
    public const string CONFIG_PATH = "Configs";

    public const string GENERATOR_CONFIG_FILENAME = "GeneratorConfig";
    public const string SPEED_UP_HERO_CONFIG_FILENAME = "SpeedUpHeroConfig";

    private static GeneratorConfig _generatorConfig;

    public static GeneratorConfig GeneratorConfig
    {
        get
        {
            if (_generatorConfig == null)
            {
                _generatorConfig = Load<GeneratorConfig>(CONFIG_PATH, GENERATOR_CONFIG_FILENAME);
            }

            return _generatorConfig;
        }
    }

    private static SpeedUpHeroConfig _speedUpHeroConfig;

    public static SpeedUpHeroConfig SpeedUpHeroConfig
    {
        get
        {
            if (_speedUpHeroConfig == null)
            {
                _speedUpHeroConfig = Load<SpeedUpHeroConfig>(CONFIG_PATH, SPEED_UP_HERO_CONFIG_FILENAME);
            }

            return _speedUpHeroConfig;
        }
    }

    public static T Load<T>(string path, string fileName) where T : Object
    {
        T file = Resources.Load<T>($"{path}/{fileName}");

        if (file == null)
        {
            Debug.LogError("Not found " + typeof(T) + " with name = " + fileName + "; Path = " + path);
        }

        return file;
    }
}