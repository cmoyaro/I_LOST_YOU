using UnityEngine;

// Este estado simula que el enemigo busca al jugador después de perderlo de vista.

public class SearchState : IEnemyState
{
    private Enemy enemy;
    private float searchTime = 5f; // Tiempo máximo buscando
    private float timer = 0f;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        timer = 0f;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        // Aquí se podría poner una animación o una búsqueda aleatoria

        // Si lo ve otra vez, lo persigue
        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(new ChaseState());
        }

        // Si se acaba el tiempo de búsqueda, vuelve a patrullar
        if (timer >= searchTime)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit() { }
}
