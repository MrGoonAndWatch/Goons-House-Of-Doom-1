using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public Weapon EquipedWeapon;
    public Animator PlayerAnimator;
    public GameObject GameOverUi;

    public double Health;
    public const double MaxHealth = 200.22;

    public bool MenuOpened;
    public bool Reading;
    public bool LockMovement;
    public bool QuickTurning;
    public bool TakingDamage;
    public bool Aiming;
    public bool Shooting;
    public bool HasSaveUiOpen;
    public bool Paused;

    public List<int> KilledEnemies;
    public List<GlobalEvent> TriggeredEvents;
    public List<int> GrabbedItems;
    public List<int> DoorsUnlocked;

    [Tooltip("Time (in seconds) between when player hp reaches 0 and when the game over screen comes up.")]
    public float GameOverUiDelay = 6.0f;
    private float _timeUntilShowGameOverUi;
    private bool _showingGameOverUi;

    [Tooltip("Time (in seconds) from when a player gets hit to when they can get hit again.")]
    public float HitCooldown = 1.0f;
    private float _remainingHitCooldown;

    private static PlayerStatus _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        DontDestroyOnLoad(gameObject);
        KilledEnemies = new List<int>();
        TriggeredEvents = new List<GlobalEvent>();
        GrabbedItems = new List<int>();
        DoorsUnlocked = new List<int>();
    }

    private void Start()
    {
        Health = MaxHealth;
    }

    private void Update()
    {
        ProcessGameOverUi();
        ProcessExitInput();
        ProcessHitCooldown();
    }

    private void ProcessGameOverUi()
    {
        if (Health > 0 || _timeUntilShowGameOverUi <= 0)
            return;

        _timeUntilShowGameOverUi -= Time.deltaTime;

        if (_timeUntilShowGameOverUi <= 0)
        {
            EnableGameOverUi();
        }
    }

    public void ForceGameOverUi()
    {
        EnableGameOverUi();
    }

    private void EnableGameOverUi()
    {
        var hordeModeManager = FindObjectOfType<HordeModeManager>();
        if (hordeModeManager == null)
        {
            GameOverUi.SetActive(true);
            _showingGameOverUi = true;
        }
        else
        {
            hordeModeManager.OnGameEnd();
        }
    }

    private void ProcessExitInput()
    {
        if (!_showingGameOverUi)
            return;

        if(Input.GetButtonDown(GameConstants.Controls.Pause))
            Application.Quit();
    }

    private void ProcessHitCooldown()
    {
        if (_remainingHitCooldown <= 0)
            return;

        _remainingHitCooldown -= Time.deltaTime;
        if (_remainingHitCooldown <= 0)
            TakingDamage = false;
    }

    public void KillEnemy(int enemyId)
    {
        KilledEnemies.Add(enemyId);
    }

    public void UnlockDoor(int doorId)
    {
        DoorsUnlocked.Add(doorId);
    }

    public void GrabItem(int itemId)
    {
        GrabbedItems.Add(itemId);
    }

    public void TriggeredEvent(GlobalEvent eventTriggered)
    {
        TriggeredEvents.Add(eventTriggered);
    }

    public void HitByAttack(double damage, string hitAnimationVariable)
    {
        if (GetHealthStatus() == HealthStatus.Dead)
            return;

        SoundManager.PlayHitSfx();
        TakingDamage = true;
        AddHealth(-damage);
        PlayerAnimator.SetBool(hitAnimationVariable, true);
        // TODO: Instead of a hard coded cooldown should have event handling from the animator to check when hittable again.
        _remainingHitCooldown = HitCooldown;
    }

    public void AddHealth(double value)
    {
        Health = Math.Max(0, Math.Min(MaxHealth, Health + value));
        HandleDeath();
    }

    public void SetHealth(double value)
    {
        Health = value;
        HandleDeath();
    }

    private void HandleDeath()
    {
        if (Health > 0)
            return;

        SoundManager.PauseSong();

        _timeUntilShowGameOverUi = GameOverUiDelay;

        if (MenuOpened)
        {
            var inv = FindObjectOfType<ToggleInventory>();
            inv.ToggleMenu();
        }

        if (Reading)
        {
            var textReader = FindObjectOfType<TextReader>();
            textReader.ForceCloseTextbox();
        }

        SoundManager.PlayDeathSfx();
        PlayerAnimator.SetBool(AnimationVariables.Player.Dead, true);
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (EquipedWeapon == weapon)
            EquipedWeapon = null;
        else
            EquipedWeapon = weapon;

        var layerIndex = PlayerAnimator.GetLayerIndex(AnimationLayers.Player.EquipLayer);
        var weight = EquipedWeapon == null ? 0 : 1;
        PlayerAnimator.SetLayerWeight(layerIndex, weight);
    }

    public HealthStatus GetHealthStatus()
    {
        if(Health == 0)
            return HealthStatus.Dead;
        if(Health <= 1)
            return HealthStatus.Special;
        if(Health <= 40)
            return HealthStatus.SpeedyBoi;
        if(Health <= 80)
            return HealthStatus.BadTummyAche;
        if(Health <= 120)
            return HealthStatus.TummyAche;
        return HealthStatus.Healthy;
    }

    public bool CanPause()
    {
        return !LockMovement && Health > 0;
    }

    public bool CanOpenMenu()
    {
        return !Paused && !Reading && Health > 0 && !TakingDamage && !HasSaveUiOpen && !LockMovement;
    }

    public bool IsMovementPrevented()
    {
        return Paused || MenuOpened || LockMovement || TakingDamage || Shooting || Reading || HasSaveUiOpen ||  Health <= 0;
    }

    public bool CanInteract()
    {
        return !Paused && !MenuOpened && !Reading && !TakingDamage && !Shooting && Health > 0;
    }

    public bool CanShoot()
    {
        return !Paused && !MenuOpened && !Reading && !TakingDamage && Health > 0 && !HasSaveUiOpen && !Reading && !LockMovement;
    }
}
