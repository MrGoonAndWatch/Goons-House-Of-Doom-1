using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    public PlayerStatus PlayerStatus;

    void OnTriggerEnter(Collider c)
    {
        if (PlayerStatus.TakingDamage)
            return;

        var enemyAttack = c.GetComponent<EnemyAttack>();
        if (enemyAttack == null)
            return;

        ProcessDamage(enemyAttack);
    }

    void OnCollisionEnter(Collision c)
    {
        if (PlayerStatus.TakingDamage)
            return;

        var enemyAttack = c.collider.GetComponent<EnemyAttack>();
        if (enemyAttack == null)
            return;

        ProcessDamage(enemyAttack);
    }

    private void ProcessDamage(EnemyAttack enemyAttack)
    {
        SoundManager.PlayHitSfx();
        PlayerStatus.TakingDamage = true;
        PlayerStatus.AddHealth(-enemyAttack.Damage);
    }

    void Update()
    {
        if (PlayerStatus.TakingDamage)
        {
            if (PlayerStatus.Health > 0)
            {
                // TODO: Do hit animation.
            }
            else
            {
                // TODO: Do death animation.
            }

            // TODO: Check if animation is done.
            PlayerStatus.TakingDamage = false;
        }
    }
}
