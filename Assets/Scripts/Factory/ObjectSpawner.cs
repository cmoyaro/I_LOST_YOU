using UnityEngine;

// Este script se encarga de generar objetos usando la fábrica.
// Está pensado para ser flexible: puede generar cualquier tipo de objeto que se le asigne a través de la interfaz IFactory.
// Lo usamos, por ejemplo, para crear "gusanos" cuando abrimos una puerta, pero también puede servir para otros elementos.
public class ObjectSpawner : MonoBehaviour
{
    public MonoBehaviour factoryComponent;   // Aquí arrastramos en el inspector la fábrica que implementa IFactory (por ejemplo, ObjectFactory)
    private IFactory<GameObject> factory; // Guardamos la referencia a la interfaz para mantener el código desacoplado.

    public int objectCount = 5;              // Número de objetos a crear cuando toque.
    public Vector3 spawnAreaCenter;          // Centro del área donde se generarán los objetos.
    public Vector3 spawnAreaSize;            // Tamaño total del área para spawnear (en X y Z).

    private bool hasSpawned = false;         // Para evitar que se vuelva a ejecutar más de una vez.

    void Awake()
    {
        // Al iniciar, comprobamos que el componente que hemos arrastrado en el inspector implementa IFactory.
        factory = factoryComponent as IFactory<GameObject>;

    }

    // Este método se puede llamar (por ejemplo, desde la puerta) para intentar generar los objetos.
    // Hace un random de 0 o 1 para simular una especie de "cara o cruz" y, si sale cara, los genera.
    [ContextMenu("Spawn Objects (Debug)")]
    public void TrySpawnObjects()
    {
        if (hasSpawned || factory == null) return;
        hasSpawned = true;

        int result = Random.Range(0, 2);  //  (cara o cruz)

        if (result == 0)
        {
            Debug.Log("Cara: generando objetos.");

            for (int i = 0; i < objectCount; i++)
            {
                Vector3 randomPos = GetRandomPosition();
                factory.Create(randomPos);
            }
        }
        else
        {
            Debug.Log("Cruz: no se generaron objetos.");
        }
    }

    // Calculamos una posición aleatoria dentro del área de spawn que hemos definido.
    private Vector3 GetRandomPosition()
    {
        return spawnAreaCenter + new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            0,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );
    }
}
