using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUseResult
{
    public MenuUseResult(bool decrementItem, bool closeMenu)
    {
        DecrementItem = decrementItem;
        CloseMenu = closeMenu;
    }

    public bool DecrementItem { get; }
    public bool CloseMenu { get; }
}
