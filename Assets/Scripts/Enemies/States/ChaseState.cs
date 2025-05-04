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
        // Perseguimos al jugador
        enemy.MoveTo(enemy.Player.position);



        Debug.Log("Persiguiendo al jugador");

        // Si está muy cerca, atacamos
        if (enemy.IsPlayerClose(2f))
        {
            enemy.ChangeState(new AttackState());
        }

        // Si ya no ve al jugador, pasa a buscarlo
        if (!enemy.CanSeePlayer())
        {
            enemy.ChangeState(new SearchState());
        }
    }

    public void Exit() { }
}
