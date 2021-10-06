using UnityEngine;

public class ItemCursor : MonoBehaviour
{
    public RectTransform RectTransform;

    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }
}
