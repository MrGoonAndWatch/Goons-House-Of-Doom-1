using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public Camera Camera;
    public GameObject Target;

    void Start()
    {
        if(Camera == null)
            Camera = FindObjectOfType<Camera>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStatus>() == null)
            return;

        Camera.transform.position = Target.transform.position;
        Camera.transform.rotation = Target.transform.rotation;
    }
}
