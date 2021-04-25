using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    
    public enum ItemType {
        None,
        Torch,
        Shield,
        Sword
    }
    
    public enum ObstacleType
    {
        None,
        Turret
    }
    
    public void SpawnObject(DropInfo dropInfo, Vector3 position)
    {
        if (dropInfo.ItemType != ItemType.None)
        {
            SpawnItem(dropInfo.ItemType);
            return;
        }

        if (dropInfo.ObstacleType != ObstacleType.None)
        {
            SpawnObstacle(dropInfo.gameObject, position);
            return;
        }
        
        Debug.LogError("Not found type for spawn in DropInfo = " + dropInfo);
    }

    private void SpawnItem(ItemType type)
    {
        switch (type) {
            case ItemType.Torch:
                _item = new Torch();
                break;
            case ItemType.Shield:
                _item = new Shield();
                break;
            case ItemType.Sword:
                _item = new Sword();
                break;
        }
        
        _item.Spawn(transform);
    }

    private void SpawnObstacle(GameObject prefab, Vector3 position)
    {
        var obstacleGo = Instantiate(prefab);
        obstacleGo.transform.position = position;
    }

    private IItem _item;

    private void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Hero")) return;
        _item.Collect();
        
        EventManager.HandleOnItemCollect(_item);
        Destroy(gameObject);
    }
}