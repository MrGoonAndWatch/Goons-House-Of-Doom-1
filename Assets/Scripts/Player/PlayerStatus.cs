using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public bool MenuOpened;
    public bool LockMovement;
    public bool QuickTurning;

    public bool IsMovementPrevented()
    {
        return MenuOpened || LockMovement;
    }
}
