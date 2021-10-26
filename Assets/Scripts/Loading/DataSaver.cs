using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSaver : MonoBehaviour
{
    private GameState _gameState;

    void Awake()
    {
        _gameState = new GameState
        {
            DeadEnemies = new int[0],
            Inventory = null,
            Health = PlayerStatus.MaxHealth,
            SceneLoadData = new SceneLoadData
            {
                TargetScene = SceneManager.GetActiveScene().name,
                LoadPosition = null,
                LoadRotation = null,
            },
            EquipedWeaponIndex = null,
        };
    }

    public GameState GetGameState()
    {
        return _gameState;
    }

    public void SaveGameStateFromScene(PlayerStatus playerStatus, PlayerInventory playerInventory, SceneLoadData sceneLoadData)
    {
        SavePlayerStatus(playerStatus, playerInventory);
        SaveInventory(playerInventory);
        SaveSceneLoadData(sceneLoadData);
    }

    private void SavePlayerStatus(PlayerStatus playerStatus, PlayerInventory playerInventory)
    {
        _gameState.Health = playerStatus.Health;
        _gameState.DeadEnemies = _gameState.DeadEnemies.Union(playerStatus.KilledEnemies).Distinct().ToArray();
        _gameState.DoorsUnlocked = _gameState.DoorsUnlocked.Union(playerStatus.DoorsUnlocked).Distinct().ToArray();
        _gameState.GrabbedItems = _gameState.GrabbedItems.Union(playerStatus.GrabbedItems).Distinct().ToArray();
        _gameState.TriggeredEvents = _gameState.TriggeredEvents.Union(playerStatus.TriggeredEvents.Select(e => (int)e)).Distinct().ToArray();
        if(playerStatus.EquipedWeapon != null)
            for (var i = 0; i < playerInventory.Items.Length; i++)
                if (playerInventory.Items[i].Item != null && playerStatus.EquipedWeapon.GetInstanceID() == playerInventory.Items[i].Item.GetInstanceID())
                    _gameState.EquipedWeaponIndex = i;
    }

    private void SaveInventory(PlayerInventory playerInventory)
    {
        var inventoryData = new List<ItemState>();
        foreach (var itemSlot in playerInventory.Items)
        {
            ItemState itemState;
            if (itemSlot.Item == null)
                itemState = new ItemState();
            else
                itemState = new ItemState
                {
                    ItemType = itemSlot.Item.GetPrefabPath(),
                    Qty = (itemSlot.Item is Weapon) ? (itemSlot.Item as Weapon).Ammo : itemSlot.Qty,
                };
            inventoryData.Add(itemState);
        }

        _gameState.Inventory = inventoryData.ToArray();
    }

    private void SaveSceneLoadData(SceneLoadData sceneLoadData)
    {
        _gameState.SceneLoadData = sceneLoadData;
    }

    public void LoadGameStateFromFileData(GameState data)
    {
        _gameState = data;
    }

    public void LoadFromGameState(PlayerStatus playerStatus, PlayerInventory playerInventory)
    {
        LoadInventory(playerInventory);
        LoadPlayerStatus(playerStatus, playerInventory);
    }

    public SceneLoadData GetSceneLoadData()
    {
        return _gameState.SceneLoadData;
    }

    private void LoadPlayerStatus(PlayerStatus playerStatus, PlayerInventory playerInventory)
    {
        playerStatus.SetHealth(_gameState.Health);
        if(_gameState.EquipedWeaponIndex.HasValue)
        {
            var targetWeapon = playerInventory.Items[_gameState.EquipedWeaponIndex.Value].Item as Weapon;
            if (targetWeapon != null)
            {
                playerStatus.EquipedWeapon = targetWeapon;
                playerInventory.EquipDirty = true;
            }
            else
            {
                Debug.LogWarning("Tried to equip non-weapon item from item slot #" + _gameState.EquipedWeaponIndex);
            }
        }
    }

    private void LoadInventory(PlayerInventory playerInventory)
    {
        if (_gameState.Inventory == null)
            return;
        
        for (var i = 0; i < playerInventory.Items.Length; i++)
        {
            playerInventory.ItemDirty[i] = true;
            if (string.IsNullOrEmpty(_gameState.Inventory[i].ItemType))
                continue;

            playerInventory.Items[i].Item = ItemGenerator.CreateItem(_gameState.Inventory[i].ItemType);
            playerInventory.Items[i].ItemSprite.texture = playerInventory.Items[i].Item.MenuIcon;
            playerInventory.Items[i].ItemSprite.color = Color.white;
            if (playerInventory.Items[i].Item is Weapon)
            {
                (playerInventory.Items[i].Item as Weapon).Ammo = _gameState.Inventory[i].Qty;
                playerInventory.Items[i].Qty = 1;
            }
            else
                playerInventory.Items[i].Qty = _gameState.Inventory[i].Qty;
        }
    }

    public class GameState
    {
        public SceneLoadData SceneLoadData;
        public ItemState[] Inventory;
        public int? EquipedWeaponIndex;
        public double Health;
        public int[] DeadEnemies;
        public int[] GrabbedItems;
        public int[] TriggeredEvents;
        public int[] DoorsUnlocked;
    }

    public class ItemState
    {
        public string ItemType;
        public int Qty;
    }
}

public class SceneLoadData
{
    public string TargetScene;
    public Vector3? LoadPosition;
    public Vector3? LoadRotation;
}