using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private PlayerStatus _playerStatus;

    public ToggleInventory ToggleInventory;

    public ItemSlot[] Items;
    public ItemCursor ItemCursor;
    public GameObject MenuActionRoot;
    public MenuAction[] MenuActions;
    public ItemCursor ActionCursor;
    private int _currentItemIndex;
    private bool _actionMenuOpen;
    private bool _combiningItems;

    private bool _pressingLeft;
    private bool _pressingRight;
    private bool _pressingUp;
    private bool _pressingDown;

    private int _currentActionIndex;
    private int _comboSelectionIndex;

    void Start()
    {
        _playerStatus = FindObjectOfType<PlayerStatus>();
    }

    // TODO: Call this. Use event system?
    public void OnOpenMenu()
    {
        _combiningItems = false;
        _actionMenuOpen = false;
        CloseActionMenu();
        _currentItemIndex = 0;
        UpdateItemCursorPosition();
    }

    void Update()
    {
        if (!_playerStatus.MenuOpened)
            return;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (_actionMenuOpen && !_combiningItems)
            HandleActionCursorMovement(vertical);
        else
            HandleItemCursorMovement(horizontal, vertical);

        if (Input.GetButtonDown("Action"))
            HandleConfirmPressed();

        if (Input.GetButtonDown("Aim"))
            HandleBackPressed();
    }

    void HandleConfirmPressed()
    {
        if (Items[_currentItemIndex].Item == null)
            return;

        if (_combiningItems && _comboSelectionIndex != _currentItemIndex)
            CombineItems(_comboSelectionIndex, _currentItemIndex);
        else if (_actionMenuOpen)
            DoAction();
        else if (!_combiningItems && !_actionMenuOpen)
            OpenActionMenu();
    }

    void HandleBackPressed()
    {
        if (_combiningItems)
            _combiningItems = false;
        else if (_actionMenuOpen)
            CloseActionMenu();
        else
            ToggleInventory.ToggleMenu();
    }

    void HandleActionCursorMovement(float vertical)
    {
        if (vertical > 0 && !_pressingUp)
        {
            if (_currentActionIndex == 0)
                _currentActionIndex = MenuActions.Length - 1;
            else
                _currentActionIndex--;
            UpdateActionCursorPosition();
            _pressingUp = true;
        }
        else if (vertical <= 0)
        {
            _pressingUp = false;
        }

        if (vertical < 0 && !_pressingDown)
        {
            _currentActionIndex = (_currentActionIndex + 1) % MenuActions.Length;
            UpdateActionCursorPosition();
            _pressingDown = true;
        }
        else if (vertical >= 0)
        {
            _pressingDown = false;
        }
    }

    void HandleItemCursorMovement(float horizontal, float vertical)
    {
        if (horizontal > 0 && !_pressingRight)
        {
            MoveItemCursorRight();
            _pressingRight = true;
        }
        else if (horizontal <= 0)
        {
            _pressingRight = false;
        }

        if (horizontal < 0 && !_pressingLeft)
        {
            MoveItemCursorLeft();
            _pressingLeft = true;
        }
        else if (horizontal >= 0)
        {
            _pressingLeft = false;
        }

        if (vertical > 0 && !_pressingUp)
        {
            MoveItemCursorUp();
            _pressingUp = true;
        }
        else if (vertical <= 0)
        {
            _pressingUp = false;
        }

        if (vertical < 0 && !_pressingDown)
        {
            MoveItemCursorDown();
            _pressingDown = true;
        }
        else if (vertical >= 0)
        {
            _pressingDown = false;
        }
    }

    void MoveItemCursorRight()
    {
        _currentItemIndex = (_currentItemIndex + 1) % Items.Length;
        UpdateItemCursorPosition();
    }

    void MoveItemCursorLeft()
    {
        if (_currentItemIndex == 0)
            _currentItemIndex = Items.Length - 1;
        else
            _currentItemIndex--;
        UpdateItemCursorPosition();
    }

    void MoveItemCursorUp()
    {
        if (_currentItemIndex < 2)
            _currentItemIndex = Items.Length - (2 - _currentItemIndex);
        else
            _currentItemIndex -= 2;
        UpdateItemCursorPosition();
    }

    void MoveItemCursorDown()
    {
        _currentItemIndex = (_currentItemIndex + 2) % Items.Length;
        UpdateItemCursorPosition();
    }

    void UpdateItemCursorPosition()
    {
        var targetSlot = Items[_currentItemIndex].RectTransform.position;
        ItemCursor.RectTransform.position = new Vector3(targetSlot.x, targetSlot.y, ItemCursor.RectTransform.position.z);
    }

    void UpdateActionCursorPosition()
    {
        var targetSlot = MenuActions[_currentActionIndex].RectTransform.position;
        ActionCursor.RectTransform.position = new Vector3(targetSlot.x, targetSlot.y, ActionCursor.RectTransform.position.z);
    }

    void DoAction()
    {
        var action = MenuActions[_currentActionIndex].ActionType;
        switch (action)
        {
            case MenuAction.MenuActionType.Use:
                Items[_currentItemIndex].Item.UseItem();
                CloseActionMenu();
                break;
            case MenuAction.MenuActionType.Combine:
                _combiningItems = true;
                _comboSelectionIndex = _currentItemIndex;
                break;
            case MenuAction.MenuActionType.Examine:
                // TODO: IMPLEMENT ME!!!
                throw new NotImplementedException();
                break;
            case MenuAction.MenuActionType.Discard:
                Items[_currentItemIndex].DiscardItem();
                CloseActionMenu();
                break;
        }
    }

    void CloseActionMenu()
    {
        MenuActionRoot.SetActive(false);
        _actionMenuOpen = false;
    }

    void CombineItems(int itemA, int itemB)
    {
        Items[itemA].Combine(Items[itemB]);
        CloseActionMenu();
    }

    void OpenActionMenu()
    {
        _currentActionIndex = 0;
        UpdateActionCursorPosition();
        MenuActionRoot.SetActive(true);
        _actionMenuOpen = true;
    }
}
