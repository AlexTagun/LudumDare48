using UnityEngine;

public class TurretObstacle : Obstacle
{
    private const int MaxAmmo = 1;

    private int _currentAmmo = MaxAmmo;
    
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private LayerMask hitLayerMask;

    [SerializeField] private Transform _muzzle;

    [SerializeField] private float DestroyTime = 10f;

    [SerializeField] private float _hintProjectileDistance = 1f;
    [SerializeField] private float _zDeltaForShoot = 0.1f;
    [SerializeField] private float _projectileSpawnDistance = 25;
    
    private Hero _target;
    

    private void Update()
    {
        TryShoot();
    }

    private void TryShoot()
    {
        if (!HasAmmo())
        {
            return;
        }

        if (!HasTarget())
        {
            return;
        }
        
        Shoot();
    }

    private bool HasAmmo()
    {
        return _currentAmmo > 0;
    }
    
    private bool HasTarget()
    {
        TryGetTarget();
        

        if (_target == null)
        {
            return false;
        }

        if (_hintProjectileDistance > Mathf.Abs(_target.transform.position.y - transform.position.y))
        {
            _target.SetHintProjectileActive(true);
        }
        
        if (Mathf.Abs(_target.transform.position.y - transform.position.y) > _zDeltaForShoot)
        {
            return false;
        }

        return true;
    }

    private void TryGetTarget()
    {
        if (_target != null) return;

        if (!InventoryManager.Instance.TryGetRandomHero(out var hero)) return;
        
            _target = hero;
    }

    private void Shoot()
    {
        _currentAmmo--;
        var projectileGo = Instantiate(_projectilePrefab);


        var direction = (_target.transform.right + _target.transform.right + _target.transform.up).normalized;
        var projectilePosition = _target.transform.position + direction * _projectileSpawnDistance;
        
        projectileGo.transform.position = projectilePosition;

        var projectile = projectileGo.GetComponent<Projectile>();

        projectile.SetTarget(_target.transform);
        
        _target.SetHintProjectileActive(false);
        
        TryDestroy();
    }

    private void TryDestroy()
    {
        if (HasAmmo())
        {
            return;
        }
        
        Destroy(gameObject, DestroyTime);
    }
}
