using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string GoesToRoom;
    public DoorLoadType DoorLoadType;
    public Vector3 StartAtPosition;
    public Vector3 StartAtAngle;

    public string[] LockedText;
    public string[] UnlockText;

    public KeyType LocksWith;

    private bool _unlocked;
    private TextReader _textReader;

    void Start()
    {
        _textReader = FindObjectOfType<TextReader>();
        if (LocksWith == KeyType.None)
            _unlocked = true;
    }
    
    void Update()
    {
    }

    public void Inspect()
    {
        if (_unlocked)
        {
            var sceneChanger = FindObjectOfType<SceneChanger>();
            sceneChanger.ChangeScene(GoesToRoom, StartAtPosition, StartAtAngle, DoorLoadType);
        }
        else
            _textReader.ReadText(LockedText);
    }

    public void Unlock(Key key)
    {
        if (_unlocked || key.KeyType != LocksWith)
            return;

        _unlocked = true;
        _textReader.ReadText(UnlockText);
    }
}
