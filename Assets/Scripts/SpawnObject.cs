using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    private IItem _item;

    public void SetItem(IItem item)
    {
        _item = item;
    }

    private void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Hero")) return;
        _item.Collect();
        
        EventManager.HandleOnItemCollect(_item);
        Destroy(gameObject);
    }
}
