// Interfaz genérica para la creación de objetos
// Sirve como base para aplicar el patrón Factory
// La clase que la implemente debe definir el método Create
using UnityEngine;
public interface IFactory<T>
{
    T Create(Vector3 position); // Devuelve una instancia del tipo T (por ejemplo, un GameObject)
}
