using System;
using System.Linq;
using UnityEngine;

public class WeaponInput : MonoBehaviour
{
    public Transform PlayerTransform;
    public LayerMask HitscanLayers;

    private PlayerStatus _playerStatus;
    private PlayerInventory _playerInventory;

    private float _shootCooldown;
    
    void Start()
    {
        _playerStatus = FindObjectOfType<PlayerStatus>();
        _playerInventory = FindObjectOfType<PlayerInventory>();
    }
    
    void Update()
    {
        if(_shootCooldown > 0)
            _shootCooldown -= Time.deltaTime;

        if (_playerStatus.EquipedWeapon != null && Input.GetButtonDown(GameConstants.Controls.Aim))
            _playerStatus.Aiming = true;
        else if (Input.GetButtonUp(GameConstants.Controls.Aim))
            _playerStatus.Aiming = false;

        if (_playerStatus.Aiming && Input.GetButtonDown(GameConstants.Controls.Action))
            Shoot();
    }

    void Shoot()
    {
        if (_shootCooldown > 0 || _playerStatus.EquipedWeapon == null || _playerStatus.EquipedWeapon.Ammo <= 0)
            return;

        _shootCooldown = _playerStatus.EquipedWeapon.GetRateOfFire();

        _playerStatus.EquipedWeapon.PlaySfx();

        if (_playerStatus.EquipedWeapon.IsHitscan())
        {
            RaycastHit hit;
            DamageHandler hitAsDamageHandler;
            var hitSomething = Physics.Raycast(PlayerTransform.position, PlayerTransform.forward, out hit, float.MaxValue, HitscanLayers);
            if (hitSomething && ValidTarget(hit, out hitAsDamageHandler))
                hitAsDamageHandler.HandleDamage(_playerStatus.EquipedWeapon.GetDamagePerHit());
        }
        else
        {
            // TODO: Spawn items to do non-hitscan detection.
            throw new NotImplementedException();
        }

        _playerStatus.EquipedWeapon.Ammo--;

        _playerInventory.RefreshItemUi();
    }

    static bool ValidTarget(RaycastHit hit, out DamageHandler hitAsDamageHandler)
    {
        hitAsDamageHandler = hit.transform.gameObject.GetComponent<DamageHandler>();
        return hitAsDamageHandler != null;
    }
}
