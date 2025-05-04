using UnityEngine;

// Este estado hace que el enemigo ataque al jugador si está cerca.

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

        // Si el jugador está muy lejos, volvemos a perseguirlo
        if (!enemy.IsPlayerClose(2f))
        {
            Debug.Log("¡Te veo!");
            enemy.ChangeState(new ChaseState());
            return;
        }

        // Atacamos si ha pasado el tiempo
        if (attackTimer >= attackCooldown)
        {
            Debug.Log("¡Ataque al jugador!");
            attackTimer = 0f;
            // Intentamos matar al jugador usando el patrón Observer
            TryKillPlayer();
        }
    }
     // Método que busca al jugador y le aplica la muerte 
    private void TryKillPlayer()
    {
        // Comprobamos si el jugador está cerca y accedemos a su script de muerte
        if (enemy.Player != null)
        {
            Debug.Log("Intentando acceder al script de muerte en el jugador...");
            PlayerDeath death = enemy.Player.GetComponent<PlayerDeath>();

            if (death != null)
            {
                Debug.Log("¡Script de muerte encontrado! Ejecutando Die()...");
                death.Die(); // Notificamos su muerte
            }
            else
            {
                Debug.LogWarning("No se encontró el script PlayerDeath en el jugador.");
            }
        }
    }

    public void Exit() { }
}
