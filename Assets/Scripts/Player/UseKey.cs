using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UseKey : MonoBehaviour
{
    public PlayerStatus PlayerStatus;

    private List<SimpleLock> _collidedSimpleLocks;
    private List<Door> _collidedDoors;
    
    void Start()
    {
        _collidedSimpleLocks = new List<SimpleLock>();
        _collidedDoors = new List<Door>();
    }

    void Update()
    {
        if (PlayerStatus.CanInteract() &&
            !(Input.GetButtonDown(GameConstants.Controls.Aim) || ControllerInputProcessor.PressedAim()) &&
            (Input.GetButtonDown(GameConstants.Controls.Action) || ControllerInputProcessor.PressedAction()))
        {
            if(_collidedSimpleLocks.Any())
                _collidedSimpleLocks.First().Inspect();
            else if (_collidedDoors.Any())
                _collidedDoors.First().Inspect();
        }
    }

    public void ResetState()
    {
        _collidedDoors.RemoveAll(d => true);
        _collidedSimpleLocks.RemoveAll(l => true);
    }

    public void Use(Key key)
    {
        if (_collidedSimpleLocks.Any())
            _collidedSimpleLocks.First().Unlock(key);
        else if (_collidedDoors.Any())
            _collidedDoors.First().Unlock(key);
    }

    void OnTriggerEnter(Collider c)
    {
        var simpleLock = c.GetComponent<SimpleLock>();
        if (simpleLock != null)
            _collidedSimpleLocks.Add(simpleLock);
        var door = c.GetComponent<Door>();
        if(door != null)
            _collidedDoors.Add(door);
    }

    void OnTriggerExit(Collider c)
    {
        var simpleLock = c.GetComponent<SimpleLock>();
        if (simpleLock != null)
            _collidedSimpleLocks.RemoveAll(l => l.GetInstanceID() == simpleLock.GetInstanceID());
        var door = c.GetComponent<Door>();
        if (door != null)
            _collidedDoors.RemoveAll(d => d.GetInstanceID() == door.GetInstanceID());
    }
}
