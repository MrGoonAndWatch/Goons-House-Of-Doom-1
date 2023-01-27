using System;
using UnityEngine;

[Serializable]
public class PlayerSpeed
{
    public float WalkSpeed;
    public float WalkBackwardsSpeed;
    public float RunSpeed;
    public float RotationSpeed;
    public float QuickTurnSpeed;
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerStatus PlayerStatus;
    public Animator PlayerAnimator;

    private bool _moving;

    public PlayerSpeed SpeedAtHealthy = new PlayerSpeed
    {
        WalkSpeed = 2.0f,
        WalkBackwardsSpeed = 1.0f,
        RunSpeed = 4.0f,
        RotationSpeed = 180.0f,
        QuickTurnSpeed = 7f,
    };

    public PlayerSpeed SpeedAtTummyAche = new PlayerSpeed
    {
        WalkSpeed = 1.0f,
        WalkBackwardsSpeed = 0.5f,
        RunSpeed = 2.0f,
        RotationSpeed = 120.0f,
        QuickTurnSpeed = 6f,
    };

    public PlayerSpeed SpeedAtSpeedyBoi = new PlayerSpeed
    {
        WalkSpeed = 4.0f,
        WalkBackwardsSpeed = 2.0f,
        RunSpeed = 8.0f,
        RotationSpeed = 240.0f,
        QuickTurnSpeed = 9f,
    };

    public PlayerSpeed SpeedAtSpecial = new PlayerSpeed
    {
        WalkSpeed = 10.0f,
        WalkBackwardsSpeed = 5.0f,
        RunSpeed = 20.0f,
        RotationSpeed = 450.0f,
        QuickTurnSpeed = 15f,
    };

    public float RotationDeadzone = 0.2f;
    public float MovementDeadzone = 0.1f;
    
    public float QuickTurnEndingDelta = 20.0f;

    private Vector3 _quickTurnTargetRotation = Vector3.zero;

    public float TimeBetweenFootstepSfxInSeconds = 1.5f;
    private float _timeTilNextFootstepSfx;

    void Start()
    {
        if (PlayerStatus == null)
        {
            PlayerStatus = GetComponent<PlayerStatus>();
        }
    }

    void Update()
    {
        if (PlayerStatus.IsMovementPrevented())
        {
            SetMoving(false);
            return;
        }

        if (_timeTilNextFootstepSfx > 0)
            _timeTilNextFootstepSfx -= Time.deltaTime;
        if (_moving && _timeTilNextFootstepSfx <= 0)
        {
            SoundManager.PlayFootstepSfx();
            _timeTilNextFootstepSfx = TimeBetweenFootstepSfxInSeconds / GetCurrentSpeed().WalkSpeed;
        }

        var currentSpeed = GetCurrentSpeed();

        if (PlayerStatus.QuickTurning)
        {
            ProcessQuickTurn(currentSpeed);
            SetMoving(false);
        }
        else
            ProcessNormalMovement(currentSpeed);
    }

    private void ProcessQuickTurn(PlayerSpeed currentSpeed)
    {
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, _quickTurnTargetRotation, currentSpeed.QuickTurnSpeed * Time.deltaTime);

        if (Math.Abs(transform.eulerAngles.y - _quickTurnTargetRotation.y) < QuickTurnEndingDelta)
        {
            transform.eulerAngles = _quickTurnTargetRotation;
            PlayerStatus.QuickTurning = false;
        }
    }

    private void ProcessNormalMovement(PlayerSpeed currentSpeed)
    {
        var horizontalInput = Input.GetAxis(GameConstants.Controls.HorizontalMovement);
        var verticalInput = Input.GetAxis(GameConstants.Controls.VerticalMovement);

        if (Math.Abs(horizontalInput) > RotationDeadzone)
            transform.Rotate(0, horizontalInput * currentSpeed.RotationSpeed * Time.deltaTime, 0);

        if (!PlayerStatus.Aiming)
            ProcessVerticalInput(verticalInput, currentSpeed);
        else
            SetMoving(false);
    }

    private void ProcessVerticalInput(float verticalInput, PlayerSpeed currentSpeed)
    {
        var moving = false;

        if (verticalInput > MovementDeadzone)
        {
            var speed = Input.GetButton(GameConstants.Controls.Run) || ControllerInputProcessor.IsPressingRun() ? currentSpeed.RunSpeed : currentSpeed.WalkSpeed;
            transform.localPosition += transform.forward * speed * Time.deltaTime;
            moving = true;
        }
        else if (verticalInput < -MovementDeadzone)
        {
            transform.localPosition -= transform.forward * currentSpeed.WalkBackwardsSpeed * Time.deltaTime;
            moving = true;
        }

        if ((Input.GetButtonDown(GameConstants.Controls.Run) || ControllerInputProcessor.PressedRun()) && verticalInput < 0)
        {
            _quickTurnTargetRotation = transform.eulerAngles + 180f * Vector3.up;
            _quickTurnTargetRotation = new Vector3(_quickTurnTargetRotation.x % 360, _quickTurnTargetRotation.y % 360,
                _quickTurnTargetRotation.z % 360);
            PlayerStatus.QuickTurning = true;
        }

        SetMoving(moving);
    }

    private PlayerSpeed GetCurrentSpeed()
    {
        switch (PlayerStatus.GetHealthStatus())
        {
            case HealthStatus.Special:
                return SpeedAtSpecial;
            case HealthStatus.SpeedyBoi:
                return SpeedAtSpeedyBoi;
            case HealthStatus.BadTummyAche:
            case HealthStatus.TummyAche:
                return SpeedAtTummyAche;
            default:
                return SpeedAtHealthy;
        }
    }

    private void SetMoving(bool newValue, bool updateAnyway = false)
    {
        if (_moving == newValue && !updateAnyway)
            return;

        _moving = newValue;
        PlayerAnimator.SetBool(AnimationVariables.Player.Walking, _moving);
        if (_moving)
            _timeTilNextFootstepSfx = TimeBetweenFootstepSfxInSeconds / GetCurrentSpeed().WalkSpeed;
    }
}
