using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _damageCount = 10f;
    [SerializeField] private float MaxLifeTime = 3f;

    [SerializeField] private Rigidbody _rigidbody;
    
    private Vector3 _direction;

    private float _currentLifeTime;
    
    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    private void FixedUpdate()
    {
        if (_currentLifeTime >= MaxLifeTime)
        {
            Destroy(gameObject);
            return;
        }
        
        _rigidbody.MovePosition(_rigidbody.position + _direction * _speed * Time.fixedDeltaTime);

        _currentLifeTime += Time.fixedDeltaTime;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponent<Health>();

        if (health == null)
        {
            return;
        }

        health.Damage(_damageCount);
        Destroy(gameObject);
    }
}
