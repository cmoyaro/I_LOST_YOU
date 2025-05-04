using UnityEngine;

// Esta fábrica se encarga de crear cualquier objeto que le pasemos como prefab.
// La idea es que no solo sirva para enemigos, sino para cualquier elemento que queramos generar de forma dinámica.
public class ObjectFactory : MonoBehaviour, IFactory<GameObject>
{
    [SerializeField] public GameObject prefabToSpawn;  // Prefab que queremos instanciar (puede ser un gusano, un objeto decorativo, etc.)

    // Este método crea una instancia del prefab en la posición que le indiquemos.
    public GameObject Create(Vector3 position)
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("ObjectFactory: No hay prefab asignado.");
            return null;
        }

        // Instanciamos el objeto y lo devolvemos para poder manipularlo si hiciera falta.
        GameObject obj = UnityEngine.Object.Instantiate(prefabToSpawn, position, Quaternion.identity);

        return obj;
    }
}
