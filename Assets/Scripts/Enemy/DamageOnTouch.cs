using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public float Damage;

    void OnCollisionEnter(Collision c)
    {
        DamagePlayer(c.collider);
    }

    void OnTriggerEnter(Collider c)
    {
        DamagePlayer(c);
    }

    private void DamagePlayer(Collider c)
    {
        var player = c.gameObject.GetComponent<PlayerStatus>();
        if (player == null)
            return;

        player.AddHealth(-Damage);
    }
}
