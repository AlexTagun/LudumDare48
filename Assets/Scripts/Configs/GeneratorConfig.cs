using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorConfig", menuName = "ScriptableObjects/GeneratorConfig", order = 1)]
public class GeneratorConfig : ScriptableObject
{
    public List<GeneratorData> GeneratorDatas;
}