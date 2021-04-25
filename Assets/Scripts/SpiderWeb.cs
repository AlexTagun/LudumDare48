using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour {
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Collider сollider;
    [SerializeField] private float damage;
    [SerializeField] private GameObject fireEffect;
    

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Hero")) return;

        var hero = other.GetComponent<Hero>();

        if (!(hero.GetItem() is Torch) || !hero.CanDoAction()) {
            var health = other.GetComponent<Health>();
            health.Damage(damage);
            return;
        }

        hero.SpendActionPoint();
        сollider.enabled = false;
        Die();
    }

    private void Die() {
        fireEffect.SetActive(true);
        rigidbody.isKinematic = false;
        rigidbody.AddForce(-Camera.main.transform.forward * 10, ForceMode.Impulse);
        rigidbody.AddTorque(new Vector3(200, 200, 200), ForceMode.Impulse);

        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine() {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}