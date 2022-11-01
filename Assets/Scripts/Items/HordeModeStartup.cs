using UnityEngine;

public class HordeModeStartup : MonoBehaviour
{
    private PlayerInventory _playerInventory;

    private bool _initialized;

    private void Update()
    {
        if (!_initialized)
        {
            _playerInventory = FindObjectOfType<PlayerInventory>();
            AddUnlimitedHandgun();
            StartMusic();
            _initialized = true;
        }
    }

    private void AddUnlimitedHandgun()
    {
        _playerInventory.ItemDirty[0] = true;
        _playerInventory.Items[0].Item = ItemGenerator.CreateItem(ResourceNames.UnlimitedHandgun);
        _playerInventory.Items[0].ItemSprite.texture = _playerInventory.Items[0].Item.MenuIcon;
        _playerInventory.Items[0].ItemSprite.color = Color.white;
        _playerInventory.Items[0].Item.UseItem();
    }

    private void StartMusic()
    {
        SoundManager.PlaySelfDestructSong();
    }
}
