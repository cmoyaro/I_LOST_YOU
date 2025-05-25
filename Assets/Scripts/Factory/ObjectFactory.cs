using UnityEngine;

// Fábrica genérica que permite instanciar cualquier prefab asignado desde el inspector
public class ObjectFactory : MonoBehaviour, IFactory<GameObject>
{
    [SerializeField] private GameObject prefabToSpawn;

    public GameObject Create(Vector3 position)
    {
        if (prefabToSpawn == null)
            return null;

        return Instantiate(prefabToSpawn, position, Quaternion.identity);
    }
}
