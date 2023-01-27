using UnityEngine;

public class ToggleInventory : MonoBehaviour
{
    public GameObject MenuPrefab;

    private PlayerStatus _playerStatus;

    private bool _menuEnabled;

    void Start()
    {
        _playerStatus = FindObjectOfType<PlayerStatus>();

        MenuPrefab.SetActive(false);
    }

    void Update()
    {
        if (_playerStatus.CanOpenMenu() &&
            (Input.GetButtonDown(GameConstants.Controls.Menu) || ControllerInputProcessor.PressedMenu()))
            ToggleMenu();
    }

    public void ToggleMenu()
    {
        _menuEnabled = !_menuEnabled;
        _playerStatus.MenuOpened = _menuEnabled;

        MenuPrefab.SetActive(_menuEnabled);

        if(_menuEnabled)
            FindObjectOfType<PlayerInventory>().OnOpenMenu();
    }
}
