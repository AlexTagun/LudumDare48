using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    public enum ObstacleType
    {
        None,
        Turret,
        Enemy,
        Coin,
        Web,
        SecretShop
    }
    
    public GameObject SpawnObject(DropInfo dropInfo, Vector3 position)
    {
        if (dropInfo.ItemType != ItemType.None)
        {
            return SpawnItem(dropInfo.ItemType, position);
        }

        if (dropInfo.ObstacleType != ObstacleType.None)
        {
            return SpawnObstacle(dropInfo.Prefab, position);
        }
        
        Debug.LogError("Not found type for spawn in DropInfo = " + dropInfo);
        return null;
    }

    private GameObject SpawnItem(ItemType type, Vector3 position)
    {
        IItem item = ItemFactory.Create(type);

        var itemGo = item.Spawn(position);
        return itemGo;
    }

    private GameObject SpawnObstacle(GameObject prefab, Vector3 position)
    {
        var obstacleGo = Instantiate(prefab);
        obstacleGo.transform.position = position;
        return obstacleGo;
    }
}