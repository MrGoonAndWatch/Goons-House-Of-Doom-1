using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public Vector3 Direction;
    public float Force = 1.0f;

    private List<Rigidbody> _pushObjects;

    void Start()
    {
        _pushObjects = new List<Rigidbody>();
    }
    
    void Update()
    {
        foreach (var pushObject in _pushObjects)
        {
            pushObject.AddForce(Direction * Force * Time.deltaTime, ForceMode.VelocityChange);
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.collider.GetComponent<PlayerStatus>())
            return;

        var rigidBody = c.collider.GetComponent<Rigidbody>();
        if(rigidBody != null)
            _pushObjects.Add(rigidBody);
    }

    void OnCollisionExit(Collision c)
    {
        var instanceId = c.collider.gameObject.GetInstanceID();
        _pushObjects.RemoveAll(p => p.GetInstanceID() == instanceId);
    }
}
