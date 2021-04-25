using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    public Transform ShootPoint => _shootPoint;

    [SerializeField] private GameObject _hintProjectile;
    
    public int MaxActionPoints;
    public int CurActivePoints;
    
    private IItem _item;

    public void SetItem(IItem item) {
        _item = item;
        if (_item != null) _item.Equip(transform);
    }

    public IItem GetItem() {
        return _item;
    }

    public bool CanDoAction() {
        return CurActivePoints > 0;
    }

    public void SpendActionPoint() {
        CurActivePoints--;
        if (CurActivePoints < 0) CurActivePoints = 0;
        EventManager.HandleOnItemSwapped();
    }

    public void SetHintProjectileActive(bool isActive)
    {
        if (_hintProjectile.activeSelf != isActive)
        {
            _hintProjectile.SetActive(isActive);
        }
    }

}