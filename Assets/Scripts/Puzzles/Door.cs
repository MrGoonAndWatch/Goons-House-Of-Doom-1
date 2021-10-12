using System;
using UnityEngine;

public class Door : MonoBehaviour
{
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
            // TODO: Load room.
            throw new NotImplementedException();
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
