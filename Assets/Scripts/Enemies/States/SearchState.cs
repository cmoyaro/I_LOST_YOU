using UnityEngine;

// Estado en el que el enemigo busca al jugador durante unos segundos tras perderlo de vista
public class SearchState : IEnemyState
{
    private Enemy enemy;
    private float searchTime = 5f;
    private float timer = 0f;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        timer = 0f;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        // Si ve al jugador durante la búsqueda, vuelve a perseguirlo
        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(new ChaseState());
        }

        // Si se agota el tiempo, vuelve a patrullar
        if (timer >= searchTime)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit() { }
}
