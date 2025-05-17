using UnityEngine;

// Este estado hace que el enemigo persiga al jugador si lo ha detectado.

public class ChaseState : IEnemyState
{
    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;

        // Cambiamos a velocidad de persecución asesina. PIPIRIPIII-PIPIRIPIII
        enemy.agent.speed = enemy.chaseSpeed;
    }

    public void Update()
    {
        Debug.Log("Persiguiendo al jugador");

        // Perseguimos al jugador
        enemy.MoveTo(enemy.Player.position);

        // Apagamos la linterna y bloqueamos su uso mientras el jugador está siendo perseguido
        FirstPersonController fpc = enemy.Player.GetComponent<FirstPersonController>();
        if (fpc != null)
        {
            if (fpc.IsFlashlightOn())
            {
                fpc.TurnOffFlashlight();
                Debug.Log("Enemigo: Apagando linterna del jugador durante persecución.");
            }

            fpc.DisableFlashlight(); // No puede volver a encenderla mientras lo persiguen
        }

        // Si está muy cerca, cambiamos a estado de ataque
        if (enemy.IsPlayerClose(2f))
        {
            enemy.ChangeState(new AttackState());
        }

        // Si ya no ve al jugador, cambia a búsqueda
        if (!enemy.CanSeePlayer())
        {
            enemy.ChangeState(new SearchState());
        }
    }


    public void Exit()
    {
        // Permitimos que el jugador vuelva a usar la linterna al salir del estado de persecución
        FirstPersonController fpc = enemy.Player.GetComponent<FirstPersonController>();
        if (fpc != null)
        {
            fpc.EnableFlashlight();
            Debug.Log("Enemigo: Se ha detenido la persecución. El jugador puede volver a usar la linterna.");
        }

        enemy.hasDisabledFlashlight = false;
    }
}
