using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InspectObject : MonoBehaviour
{
    public TextReader TextReader;

    private PlayerStatus _playerStatus;

    private List<Inspectable> _touchingInspectables;

    void Start()
    {
        _playerStatus = FindObjectOfType<PlayerStatus>();
        _touchingInspectables = new List<Inspectable>();
    }
    
    void Update()
    {
        if(_playerStatus.CanInteract() && Input.GetButtonDown(GameConstants.Controls.Action))
            ProcessInspect();
    }

    void ProcessInspect()
    {
        if (!_touchingInspectables.Any(i => i.CanInspect()))
            return;

        var targetInspectable = _touchingInspectables.Last(i => i.CanInspect());
        TextReader.ReadText(targetInspectable.InspectText, targetInspectable.Choices, () => targetInspectable.OnUsed());
    }

    void OnTriggerEnter(Collider c)
    {
        var item = c.GetComponent<Inspectable>();
        if (item == null)
            return;

        _touchingInspectables.Add(item);
    }

    void OnTriggerExit(Collider c)
    {
        var item = c.GetComponent<Inspectable>();
        if (item == null)
            return;

        _touchingInspectables.RemoveAll(i => i.GetInstanceID() == item.GetInstanceID());
    }

    void OnCollisionEnter(Collision c)
    {
        var item = c.collider.GetComponent<Inspectable>();
        if (item == null)
            return;

        _touchingInspectables.Add(item);
    }

    void OnCollisionExit(Collision c)
    {
        var inspectable = c.collider.GetComponent<Inspectable>();
        if (inspectable == null)
            return;

        _touchingInspectables.RemoveAll(i => i.GetInstanceID() == inspectable.GetInstanceID());
    }
}
