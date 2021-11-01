using UnityEngine;

public class SimpleLock : MonoBehaviour
{
    public string[] LockedText;
    public string[] UnlockText;
    public string LootedText = "There's nothing else inside.";
    public KeyType UnlocksWith;
    public Item ContainsItem;

    private bool _unlocked;
    private bool _looted;
    private TextReader _textReader;
    private PlayerInventory _playerInventory;

    void Start()
    {
        _textReader = FindObjectOfType<TextReader>();
        _playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public virtual void Inspect()
    {
        if (_looted)
            _textReader.ReadText(new[] {LootedText});
        else if (_unlocked)
        {
            _playerInventory.AddItem(ContainsItem);
            _looted = true;
        }
        else
            _textReader.ReadText(LockedText);
    }

    public virtual void Unlock(Key key)
    {
        if (_unlocked || key.GetKeyType() != UnlocksWith)
            return;

        _unlocked = true;
        _textReader.ReadText(UnlockText);
    }
}
