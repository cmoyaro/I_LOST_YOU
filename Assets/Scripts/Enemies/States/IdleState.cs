using UnityEngine;

// Estado inicial del enemigo. Puede servir como punto de espera o inactividad. 
//Por el momento no se esta usando

public class IdleState : IEnemyState
{
    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Update()
    {
        // Si ve al jugador, lo persigue
        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(new ChaseState());
        }
    }

    public void Exit() { }
}
