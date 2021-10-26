using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int EnemyId;

    public float SightRange = 10.0f;
    public LayerMask LayerMask;

    public PlayerStatus PlayerStatus;

    protected virtual void Start()
    {
        if (PlayerStatus == null)
            PlayerStatus = FindObjectOfType<PlayerStatus>();
    }

    protected float DistanceToPlayer()
    {
        return Vector3.Distance(gameObject.transform.position, PlayerStatus.transform.position);
    }

    protected bool CanSeePlayer()
    {
        RaycastHit hit;
        var direction = PlayerStatus.transform.position - gameObject.transform.position;
        if (Physics.Raycast(gameObject.transform.position, direction, out hit, SightRange, LayerMask))
            return hit.collider.GetComponent<PlayerStatus>() != null;

        return false;
    }
}
