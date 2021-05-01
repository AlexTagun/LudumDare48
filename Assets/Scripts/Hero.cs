using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Collider сollider;

    [SerializeField] private ParticleSystem damageEffect;

    [SerializeField] private AudioSource _heroDeadSound;
    [SerializeField] private AudioSource _potionUseSound;

    [SerializeField] public CharacterMovement CharacterMovement;
    
    public Transform ShootPoint => _shootPoint;
    public Health Health;

    [SerializeField] private GameObject _hintProjectile;
    
    public int MaxActionPoints;
    public int CurActivePoints;

    private bool _isHintedByProjectile;
    public bool IsHintedByProjectile
    {
        get => _isHintedByProjectile;
        set => _isHintedByProjectile = value;
    }

    private IItem _item;

    private void Awake()
    {
        Health.OnGetDamage += OnGetDamage;
        EventManager.OnHpEnded += OnHpEnded;
    }

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

    public void Kill() {
        сollider.enabled = false;
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.AddForce(-Camera.main.transform.forward * 10, ForceMode.Impulse);
        rigidbody.AddTorque(new Vector3(200, 200,200), ForceMode.Impulse);

        StartCoroutine(DestroyCoroutine());
    }
    
    private IEnumerator DestroyCoroutine() {
        yield return new WaitForSeconds(4);
        Health.OnGetDamage -= OnGetDamage;
        EventManager.OnHpEnded -= OnHpEnded;
        Destroy(gameObject);
    }

    private void OnGetDamage()
    {
        damageEffect.gameObject.SetActive(true);
        damageEffect.Play(true);
    }

    private void OnHpEnded(Hero hero)
    {
        if(hero != this){return;}

        _heroDeadSound.Play();
        SetHintProjectileActive(false);
    }

    public void AddActivePoints(int value) {
        CurActivePoints += value;
        CurActivePoints = Math.Min(CurActivePoints, MaxActionPoints);
        _potionUseSound.Play();
    }
}