using UnityEngine;

public static class DataManager
{
    public const string CONFIG_PATH = "Configs";

    public const string GENERATOR_CONFIG_FILENAME = "GeneratorConfig";

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