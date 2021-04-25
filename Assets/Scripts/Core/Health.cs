using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHp = 100f;
    [SerializeField] private bool IsImmortal;

    private float _currentHp;

    

    private void Awake()
    {
        Init();
    }

    private void Init()
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

        if (0 >= _currentHp)
        {
            EventManager.HandleOnHpEnded(GetComponent<Hero>());
        }
    }

    public void Heal(float points)
    {
        _currentHp = Mathf.Min(_maxHp, _currentHp + points);
    }
}
