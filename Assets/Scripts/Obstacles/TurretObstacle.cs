using UnityEngine;

public class TurretObstacle : Obstacle
{
    private const int MaxAmmo = 1;

    private int _currentAmmo = MaxAmmo;
    
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private LayerMask hitLayerMask;

    [SerializeField] private Transform _muzzle;

    [SerializeField] private float DestroyTime = 10f;

    [SerializeField] private float _hintProjectileDistance = 0.5f;
    
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
        
        if (_target.transform.position.y > transform.position.y)
        {
            return false;
        }
        
        var directionToTarget = (_target.transform.position - _muzzle.transform.position).normalized;
        
        if (!Physics.Raycast(_muzzle.transform.position, directionToTarget, out var targetHitPoint, 100f)) return false;
        
        if (!targetHitPoint.transform == _target)
        {
            return false;
        }
        
        
        var direction = transform.forward;

        if (!Physics.Raycast(transform.position, direction, out var shootHitPoint, 100f, hitLayerMask.value))
        {
            return false;
        }
        
        transform.LookAt(_target.ShootPoint);

        return _target.transform == shootHitPoint.transform;
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
        projectileGo.transform.position = _muzzle.transform.position;

        var projectile = projectileGo.GetComponent<Projectile>();

        projectile.SetTarget(_target.ShootPoint);
        
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
