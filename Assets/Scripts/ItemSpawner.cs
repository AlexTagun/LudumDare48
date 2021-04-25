using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    public enum ObstacleType
    {
        None,
        Turret,
        Enemy
    }
    
    public void SpawnObject(DropInfo dropInfo, Vector3 position)
    {
        if (dropInfo.ItemType != ItemType.None)
        {
            SpawnItem(dropInfo.ItemType, position);
            return;
        }

        if (dropInfo.ObstacleType != ObstacleType.None)
        {
            SpawnObstacle(dropInfo.Prefab, position);
            return;
        }
        
        Debug.LogError("Not found type for spawn in DropInfo = " + dropInfo);
    }

    private void SpawnItem(ItemType type, Vector3 position)
    {
        Item item = null; 
        
        switch (type) {
            case ItemType.Torch:
                item = new Torch();
                break;
            case ItemType.Shield:
                item = new Shield();
                break;
            case ItemType.Sword:
                item = new Sword();
                break;
        }
        
        item.Spawn(position);
    }

    private void SpawnObstacle(GameObject prefab, Vector3 position)
    {
        var obstacleGo = Instantiate(prefab);
        obstacleGo.transform.position = position;
    }
}