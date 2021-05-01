using UnityEngine;

public class TurretObstacle : Obstacle
{
    [SerializeField] private GameObject _projectilePrefab;

    [SerializeField] private float DestroyTime = 10f;

    [SerializeField] private float _hintProjectileDistance = 1f;
    [SerializeField] private float _projectileSpawnDistance = 25;

    [SerializeField] private float _timeUntilShoot = 3;

    [SerializeField] private AudioSource _startArrowSound;
    
    private Hero _target;

    private bool _isStartShoot;
    

    private void Update()
    {
        TryStartShoot();
    }

    private void TryStartShoot()
    {
        if (_isStartShoot)
        {
            return;
        }

        UpdateTarget();
    }
    
    private void UpdateTarget()
    {
        TryGetTarget();
        

        if (_target == null)
        {
            return;
        }

        if (!CanHint())
        {
            return;
        }

        _target.SetHintProjectileActive(true);
        
        Invoke(nameof(TryShoot), _timeUntilShoot);
        _isStartShoot = true;
    }

    private bool CanHint()
    {
        return _hintProjectileDistance > Mathf.Abs(_target.transform.position.y - transform.position.y);
    }

    private void TryGetTarget()
    {
        if (_target != null) return;

        if (!InventoryManager.Instance.TryGetRandomHero(out var hero)) return;
        
            _target = hero;
            _target.IsHintedByProjectile = true;
    }

    private void TryShoot()
    {
        if (_target != null)
        {
            Shoot();
        }

        Destroy(gameObject, DestroyTime);
    }

    private void Shoot()
    {
        var projectileGo = Instantiate(_projectilePrefab);
        
        var direction = (_target.transform.right + _target.transform.right + _target.transform.up).normalized;
        var projectilePosition = _target.transform.position + direction * _projectileSpawnDistance;
        
        projectileGo.transform.position = projectilePosition;
        var projectile = projectileGo.GetComponent<Projectile>();
        projectile.SetTarget(_target.transform);
        
        _startArrowSound.Play();
    }
}
