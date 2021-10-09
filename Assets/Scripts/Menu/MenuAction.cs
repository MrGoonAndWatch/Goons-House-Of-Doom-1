using UnityEngine;
using UnityEngine.UI;

public class MenuAction : MonoBehaviour
{
    public MenuActionType ActionType;
    public Text Textbox;
    public RectTransform RectTransform;

    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public enum MenuActionType
    {
        Use,
        Combine,
        Examine,
        Discard,
    }
}
