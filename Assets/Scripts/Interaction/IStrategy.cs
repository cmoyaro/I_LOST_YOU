using UnityEngine;

// Interfaz genérica para definir una estrategia.
// Aplica el patrón Strategy, permitiendo que cada objeto tenga un comportamiento configurable.
// Ideal para interacciones en el juego (recoger, abrir, activar...) sin acoplar la lógica.
public interface IStrategy
{
    void Execute(GameObject interactor); // Ejecuta la acción específica
}
