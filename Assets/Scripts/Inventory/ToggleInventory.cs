using UnityEngine;

public class ToggleInventory : MonoBehaviour
{
    public GameObject MenuPrefab;

    private bool _menuEnabled;

    void Start()
    {
        MenuPrefab.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            _menuEnabled = !_menuEnabled;
            MenuPrefab.SetActive(_menuEnabled);
        }
    }
}
