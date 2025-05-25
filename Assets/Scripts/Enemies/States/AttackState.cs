using UnityEngine;

// Estado en el que el enemigo intenta atacar al jugador si está cerca
public class AttackState : IEnemyState
{
    private Enemy enemy;
    private float attackCooldown = 2f;
    private float attackTimer = 0f;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        attackTimer = 0f;
    }

    public void Update()
    {
        attackTimer += Time.deltaTime;

        // Si el jugador ya no está cerca, volvemos a perseguirlo
        if (!enemy.IsPlayerClose(2f))
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        // Ataca si ha pasado el tiempo de recarga
        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            TryKillPlayer();
        }
    }

    // Si el jugador tiene el script PlayerDeath, se le aplica la muerte
    private void TryKillPlayer()
    {
        if (enemy.Player != null)
        {
            PlayerDeath death = enemy.Player.GetComponent<PlayerDeath>();
            if (death != null)
            {
                death.Die();
            }
        }
    }

    public void Exit() { }
}
