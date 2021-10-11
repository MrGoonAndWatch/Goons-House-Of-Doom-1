using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStatus PlayerStatus;

    public float WalkSpeed = 2.0f;
    public float WalkBackwardsSpeed = 1.0f;
    public float RunSpeed = 4.0f;

    public float RotationDeadzone = 0.2f;
    public float MovementDeadzone = 0.1f;

    public float QuickTurnSpeed = 7f;
    public float RotationSpeed = 70.0f;
    public float QuickTurnEndingDelta = 20.0f;

    private Vector3 _quickTurnTargetRotation = Vector3.zero;

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
            return;

        if (PlayerStatus.QuickTurning)
            ProcessQuickTurn();
        else
            ProcessNormalMovement();
    }

    private void ProcessQuickTurn()
    {
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, _quickTurnTargetRotation, QuickTurnSpeed * Time.deltaTime);

        if (Math.Abs(transform.eulerAngles.y - _quickTurnTargetRotation.y) < QuickTurnEndingDelta)
        {
            transform.eulerAngles = _quickTurnTargetRotation;
            PlayerStatus.QuickTurning = false;
        }
    }

    private void ProcessNormalMovement()
    {
        var horizontalInput = Input.GetAxis(GameConstants.Controls.HorizontalMovement);
        var verticalInput = Input.GetAxis(GameConstants.Controls.VerticalMovement);

        if (Math.Abs(horizontalInput) > RotationDeadzone)
            transform.Rotate(0, horizontalInput * RotationSpeed * Time.deltaTime, 0);

        if(!PlayerStatus.Aiming)
            ProcessVerticalInput(verticalInput);
    }

    private void ProcessVerticalInput(float verticalInput)
    {
        if (verticalInput > MovementDeadzone)
        {
            var speed = Input.GetButton(GameConstants.Controls.Run) ? RunSpeed : WalkSpeed;
            transform.localPosition += transform.forward * speed * Time.deltaTime;
        }
        else if (verticalInput < -MovementDeadzone)
        {
            transform.localPosition -= transform.forward * WalkBackwardsSpeed * Time.deltaTime;
        }

        if (Input.GetButtonDown(GameConstants.Controls.Run) && verticalInput < 0)
        {
            _quickTurnTargetRotation = transform.eulerAngles + 180f * Vector3.up;
            _quickTurnTargetRotation = new Vector3(_quickTurnTargetRotation.x % 360, _quickTurnTargetRotation.y % 360,
                _quickTurnTargetRotation.z % 360);
            PlayerStatus.QuickTurning = true;
        }
    }
}
