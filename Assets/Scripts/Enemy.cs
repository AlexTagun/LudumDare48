using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float damage;

    [SerializeField] private AudioSource _swordAttack;

    private void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Hero")) return;

        var hero = other.GetComponent<Hero>();
        
        _swordAttack.Play();
        
        if (!(hero.GetItem() is Sword) || !hero.CanDoAction()) {
            var health = other.GetComponent<Health>();
            health.Damage(damage);
            Die();
            return;
        }
        
        hero.SpendActionPoint();
        Die();
    }

    private void Die() {
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.AddForce(-Camera.main.transform.forward * 10, ForceMode.Impulse);
        rigidbody.AddTorque(new Vector3(200, 200,200), ForceMode.Impulse);
        rigidbody.useGravity = true;

        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine() {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}