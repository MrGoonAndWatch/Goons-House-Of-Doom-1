using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int DoorId;

    public string GoesToRoom;
    public DoorLoadType DoorLoadType;
    public Vector3 StartAtPosition;
    public Vector3 StartAtAngle;

    public string[] LockedText;
    public string[] UnlockText;

    public KeyType LocksWith;
    public GlobalEvent UnlocksOnEvent;

    private bool _unlocked;
    private TextReader _textReader;

    void Start()
    {
        _textReader = FindObjectOfType<TextReader>();
        if (LocksWith == KeyType.None && UnlocksOnEvent == GlobalEvent.None)
            _unlocked = true;
    }

    public void Inspect()
    {
        if (_unlocked)
        {
            var sceneChanger = FindObjectOfType<SceneChanger>();
            var sceneChangeInfo = new SceneLoadData
            {
                TargetScene = GoesToRoom,
                LoadPosition = StartAtPosition,
                LoadRotation = StartAtAngle,
            };
            sceneChanger.ChangeScene(sceneChangeInfo, DoorLoadType);
        }
        else if(LockedText.Any())
            _textReader.ReadText(LockedText);
    }

    public void Unlock(Key key)
    {
        if (_unlocked || key.KeyType != LocksWith)
            return;

        _unlocked = true;
        var playerStatus = FindObjectOfType<PlayerStatus>();
        playerStatus.UnlockDoor(DoorId);
        if (UnlockText.Any())
            _textReader.ReadText(UnlockText);
    }

    public void OnEvent(GlobalEvent globalEvent)
    {
        if (_unlocked || globalEvent != UnlocksOnEvent)
            return;
        
        _unlocked = true;
        var playerStatus = FindObjectOfType<PlayerStatus>();
        playerStatus.UnlockDoor(DoorId);
    }

    public void ForceUnlock()
    {
        _unlocked = true;
    }
}
