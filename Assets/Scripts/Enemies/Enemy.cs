using UnityEngine;
using UnityEngine.AI;

// Este script gestiona el comportamiento del enemigo (el ojo flotante).
// Usa el patrón State para alternar entre patrullar, perseguir, atacar, etc.
// Además, ahora permite rotar una parte visual del enemigo (por ejemplo, su "cuerpo" o la esfera que representa el ojo).

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;             // El NavMeshAgent para moverse por el escenario
    public Transform player;               // Referencia al jugador (para detectar y perseguir)
    public Transform[] patrolPoints;       // Puntos por los que patrullará

    [Header("Velocidades")]
    public float patrolSpeed = 6f;         // Velocidad cuando patrulla
    public float chaseSpeed = 12f;          // Velocidad cuando persigue al jugador

    [Header("Detección")]
    public float visionRange = 10f;        // Rango máximo de visión
    public float visionAngle = 90f;        // Ángulo de visión (tipo cono)
    public LayerMask obstacleMask;         // Qué capas bloquean la visión

    public int currentPatrolIndex = 0;


    [Header("Cuerpo visual (opcional)")]
    public Transform bodyTransform;        // El objeto visual que queremos rotar hacia el jugador (por ejemplo, la esfera del ojo)

    private IEnemyState currentState;      // Estado actual del enemigo

    public Transform Player => player;

    [Header("Control de linterna del jugador")]
    public bool hasDisabledFlashlight = false;

    void Start()
    {
        ChangeState(new PatrolState());

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogWarning("No se encontró un objeto con la etiqueta Player.");
            }
        }
    }

    void Update()
    {
        currentState?.Update();

        // hacemos que la parte visual (el ojo) mire hacia el jugador si está cerca y lo ve
        if (bodyTransform != null && CanSeePlayer())
        {
            Vector3 dirToPlayer = (player.position - bodyTransform.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(new Vector3(dirToPlayer.x, 0, dirToPlayer.z));
            bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, lookRot, Time.deltaTime * 5f);
        }
    }

    // Cambia de estado (patrulla, persecución, ataque, etc.)
    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter(this);
    }

    // Mueve al enemigo a la posición indicada
    public void MoveTo(Vector3 targetPos)
    {
        if (agent != null)
            agent.SetDestination(targetPos);
    }

    // Devuelve los puntos de patrulla (para usar desde PatrolState)
    public Transform[] GetPatrolPoints()
    {
        return patrolPoints;
    }

    // Comprueba si el jugador está dentro del campo de visión y línea de visión (rayo)
    public bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 dirToPlayer = player.position - transform.position;
        float distToPlayer = dirToPlayer.magnitude;

        if (distToPlayer < visionRange)
        {
            float angle = Vector3.Angle(transform.forward, dirToPlayer);
            if (angle < visionAngle / 2f)
            {
                // Comprobamos si hay obstáculos entre enemigo y jugador
                if (!Physics.Raycast(transform.position, dirToPlayer.normalized, distToPlayer, obstacleMask))
                {
                    Debug.DrawLine(transform.position, player.position, Color.green);  // Línea de depuración
                    return true;
                }
            }
        }

        return false;
    }

    // Comprueba si el jugador está muy cerca (para ataque)
    public bool IsPlayerClose(float distance)
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) < distance;
    }
}
