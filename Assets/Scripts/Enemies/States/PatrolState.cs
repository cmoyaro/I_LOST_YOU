using UnityEngine;

// Este estado hace que el enemigo se mueva entre puntos predefinidos.
// Ahora reutiliza el índice global del Enemy para NO reiniciar la patrulla cada vez que entra en este estado.

public class PatrolState : IEnemyState
{
    private Enemy enemy; // Referencia al enemigo
    private Transform[] waypoints; // Lista de puntos de patrulla

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        waypoints = enemy.GetPatrolPoints();

        // Configuramos la velocidad de patrulla tranquila (modo chill)
        enemy.agent.speed = enemy.patrolSpeed;
    }

    public void Update()
    {
        if (waypoints.Length == 0) return;

        // Si estamos cerca del punto actual, pasamos al siguiente (usando el índice global)
        if (Vector3.Distance(enemy.transform.position, waypoints[enemy.currentPatrolIndex].position) < 1f)
        {
            enemy.currentPatrolIndex = (enemy.currentPatrolIndex + 1) % waypoints.Length;
        }

        // Movemos al enemigo hacia el punto actual
        enemy.MoveTo(waypoints[enemy.currentPatrolIndex].position);
        Debug.Log("Patrullando (Índice actual: " + enemy.currentPatrolIndex + ")");

        // Si ve al jugador, cambiamos al estado de persecución
        if (enemy.CanSeePlayer())
        {
            Debug.Log("Jugador detectado");
            enemy.ChangeState(new ChaseState());
        }
    }

    public void Exit()
    {
        // Por ahora no necesitamos hacer nada al salir de la patrulla
    }
}
