using System;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public float MaxHp;

    private float _hp;

    void Start()
    {
        _hp = MaxHp;
    }

    void Update()
    {
        if (_hp == 0)
        {
            // TODO: Something better.
            gameObject.SetActive(false);
        }
    }

    public void HandleDamage(float damage)
    {
        _hp = Math.Max(0, _hp - damage);
    }
}
