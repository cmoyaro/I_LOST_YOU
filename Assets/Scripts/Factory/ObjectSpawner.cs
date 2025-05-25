using UnityEngine;

// Generador de objetos que usa una fábrica externa basada en la interfaz IFactory
public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private MonoBehaviour factoryComponent; // Componente que implementa IFactory
    private IFactory<GameObject> factory;

    public int objectCount = 5;
    public Vector3 spawnAreaCenter;
    public Vector3 spawnAreaSize;

    private bool hasSpawned = false;

    void Awake()
    {
        factory = factoryComponent as IFactory<GameObject>;
    }

    // Método para intentar generar objetos. Tiene un 50% de probabilidad de hacerlo.
    [ContextMenu("Spawn Objects")]
    public void TrySpawnObjects()
    {
        if (hasSpawned || factory == null) return;

        hasSpawned = true;

        if (Random.Range(0, 2) == 0)
        {
            for (int i = 0; i < objectCount; i++)
            {
                Vector3 randomPos = GetRandomPosition();
                factory.Create(randomPos);
            }
        }
    }

    // Devuelve una posición aleatoria dentro del área de spawn definida
    private Vector3 GetRandomPosition()
    {
        return spawnAreaCenter + new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            0,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );
    }
}
