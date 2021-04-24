using UnityEngine;

public class TurretObstacle : Obstacle
{
    private const int MaxAmmo = 1;

    private int _currentAmmo = MaxAmmo;
    
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private LayerMask hitLayerMask;

    [SerializeField] private Transform _muzzle;
    

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

        if (!HasTarget(out var targetPosition))
        {
            return;
        }
        
        Shoot(targetPosition);
    }

    private bool HasAmmo()
    {
        return _currentAmmo > 0;
    }
    
    private bool HasTarget(out Vector3 targetPosition)
    {
        targetPosition = Vector3.zero;
        
        var direction = transform.forward;
        
        if (!Physics.Raycast(transform.position, direction, out var hitPoint, 100f, hitLayerMask.value))
        {
            return false;
        }

        targetPosition = hitPoint.point;
        return true;
    }

    private void Shoot(Vector3 targetPosition)
    {
        _currentAmmo--;
        var projectileGo = Instantiate(_projectilePrefab);
        projectileGo.transform.position = _muzzle.transform.position;

        var projectile = projectileGo.GetComponent<Projectile>();

        var direction = (targetPosition - projectileGo.transform.position).normalized;
        projectile.SetDirection(direction);
    }
}
