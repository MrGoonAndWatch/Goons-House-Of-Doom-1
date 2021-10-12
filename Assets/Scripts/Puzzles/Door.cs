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
            // TODO: Load position & rotation of player, maybe trigger cam change too?
            if (DoorLoadType == DoorLoadType.None)
            {
                SceneManager.LoadScene(GoesToRoom);
            }
            else
            {
                SceneManager.LoadScene(DoorLoadType.ToString());
            }
            
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
