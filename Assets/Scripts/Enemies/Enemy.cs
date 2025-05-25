using UnityEngine;
using UnityEngine.AI;

// Comportamiento general del enemigo: patrullar, detectar, perseguir y rotar visualmente hacia el jugador.
public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform[] patrolPoints;

    [Header("Velocidades")]
    public float patrolSpeed = 6f;
    public float chaseSpeed = 12f;

    [Header("Detección")]
    public float visionRange = 10f;
    public float visionAngle = 90f;
    public LayerMask obstacleMask;

    public int currentPatrolIndex = 0;

    [Header("Cuerpo visual (opcional)")]
    public Transform bodyTransform;

    private IEnemyState currentState;

    public Transform Player => player;

    [Header("Control de linterna del jugador")]
    public bool hasDisabledFlashlight = false;

    void Start()
    {
        ChangeState(new PatrolState());

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        currentState?.Update();

        // El cuerpo visual rota suavemente hacia el jugador si está a la vista
        if (bodyTransform != null && CanSeePlayer())
        {
            Vector3 dirToPlayer = (player.position - bodyTransform.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(new Vector3(dirToPlayer.x, 0, dirToPlayer.z));
            bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, lookRot, Time.deltaTime * 5f);
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter(this);
    }

    public void MoveTo(Vector3 targetPos)
    {
        if (agent != null)
            agent.SetDestination(targetPos);
    }

    public Transform[] GetPatrolPoints()
    {
        return patrolPoints;
    }

    // Comprueba si el jugador está dentro del ángulo de visión y sin obstáculos entre medias
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
                if (!Physics.Raycast(transform.position, dirToPlayer.normalized, distToPlayer, obstacleMask))
                    return true;
            }
        }

        return false;
    }

    // Comprueba si el jugador está a una distancia muy corta
    public bool IsPlayerClose(float distance)
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) < distance;
    }
}
