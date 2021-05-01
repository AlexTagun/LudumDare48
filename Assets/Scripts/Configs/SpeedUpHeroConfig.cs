using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedUpHeroConfig", menuName = "ScriptableObjects/SpeedUpHeroConfig", order = 1)]
public class SpeedUpHeroConfig : ScriptableObject
{
    public List<SpeedUpHeroData> SpeedUpHeroDatas;
}