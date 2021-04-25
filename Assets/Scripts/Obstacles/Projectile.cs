using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _damageCount = 10f;
    [SerializeField] private float MaxLifeTime = 3f;

    [SerializeField] private Rigidbody _rigidbody;
    
    private Transform _target;
    private float _currentLifeTime;

    private bool _isStopped;
    
    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void FixedUpdate()
    {
        if (_isStopped)
        {
            return;
        }
        
        if (_currentLifeTime >= MaxLifeTime)
        {
            Destroy(gameObject);
            return;
        }

        var direction = (_target.position - _rigidbody.position).normalized;
        
        _rigidbody.MovePosition(_rigidbody.position + direction * _speed * Time.fixedDeltaTime);

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

        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _isStopped = true;
        
        
        StartCoroutine(DestroyCoroutine());
    }
    
    private IEnumerator DestroyCoroutine() {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
