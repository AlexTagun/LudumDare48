using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHp = 100f;
    [SerializeField] private bool IsImmortal;
    
    [SerializeField] private AudioSource _potionUseSound;

    private float _currentHp;

    public Action OnGetDamage;

    public void Init()
    {
        _currentHp = _maxHp;
    }

    public void Damage(float points)
    {
        if (IsImmortal)
        {
            return;
        }

        _currentHp = Mathf.Max(0f, _currentHp - points);

        EventManager.HandleOnItemSwapped();
        
        OnGetDamage?.Invoke();
        
        if (0 >= _currentHp)
        {
            EventManager.HandleOnHpEnded(GetComponent<Hero>());
        }
    }

    public void Heal(float points)
    {
        _currentHp = Mathf.Min(_maxHp, _currentHp + points);
        _potionUseSound?.Play();
    }

    public float GetHpPercentage() {
        return _currentHp / _maxHp;
    }
}
