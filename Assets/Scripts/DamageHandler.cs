using System;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public int EnemyId;

    public float MaxHp;

    private float _hp;

    private bool _dead;

    private HordeModeManager _hordeModeManager;

    void Start()
    {
        _hp = MaxHp;
        _hordeModeManager = FindObjectOfType<HordeModeManager>();
    }

    void Update()
    {
        if (!_dead && _hp == 0)
        {
            _dead = true;
            if (EnemyId != 0)
            {
                var playerStatus = FindObjectOfType<PlayerStatus>();
                playerStatus.KillEnemy(EnemyId);
            }

            // TODO: Activate some animation instead.
            gameObject.SetActive(false);
            if (_hordeModeManager != null)
                _hordeModeManager.AddKill();
        }
    }

    public void ForceDead()
    {
        _hp = 0;
    }

    public void HandleDamage(float damage)
    {
        _hp = Math.Max(0, _hp - damage);
    }
}
