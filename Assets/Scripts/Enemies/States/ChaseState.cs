using UnityEngine;

// Estado en el que el enemigo persigue activamente al jugador
public class ChaseState : IEnemyState
{
    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        enemy.agent.speed = enemy.chaseSpeed;
    }

    public void Update()
    {
        enemy.MoveTo(enemy.Player.position);

        // Apaga la linterna y bloquea su uso mientras el jugador está siendo perseguido
        FirstPersonController fpc = enemy.Player.GetComponent<FirstPersonController>();
        if (fpc != null)
        {
            if (fpc.IsFlashlightOn())
                fpc.TurnOffFlashlight();

            fpc.DisableFlashlight();
        }

        // Cambia a estado de ataque si está muy cerca
        if (enemy.IsPlayerClose(2f))
        {
            enemy.ChangeState(new AttackState());
        }

        // Si pierde de vista al jugador, cambia a estado de búsqueda
        if (!enemy.CanSeePlayer())
        {
            enemy.ChangeState(new SearchState());
        }
    }

    public void Exit()
    {
        // Permite que el jugador vuelva a usar la linterna al dejar de ser perseguido
        FirstPersonController fpc = enemy.Player.GetComponent<FirstPersonController>();
        if (fpc != null)
            fpc.EnableFlashlight();

        enemy.hasDisabledFlashlight = false;
    }
}
