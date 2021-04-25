using System;
using UnityEngine;

[Serializable]
public class DropInfo
{
    public GameObject Prefab;
    public ItemSpawner.ItemType ItemType;
    public ItemSpawner.ObstacleType ObstacleType;
    
    public int Weight;

    public Vector3 SpawnOffset;
}