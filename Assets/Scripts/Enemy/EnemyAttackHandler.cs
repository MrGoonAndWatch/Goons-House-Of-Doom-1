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
        PlayerStatus.HitByAttack(enemyAttack.Damage, AnimationVariables.Player.Bitten);
    }
}
