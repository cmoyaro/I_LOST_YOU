using UnityEngine;

// Clase base que representa un objeto interactuable en el mundo del juego.
// Este objeto no define directamente qué hace al interactuar, sino que delega esa lógica
// a una estrategia externa que implementa Itrategy.
// Esto aplica el patrón Strategy, permitiendo cambiar el comportamiento sin modificar esta clase.
public class InteractableObject : MonoBehaviour
{
    [Tooltip("Script que define qué ocurre cuando se interactúa con este objeto. Debe implementar IStrategy.")]
    public MonoBehaviour strategyScript; // Componente que implementa la lógica (ej: abrir puerta, recoger objeto)

    private IStrategy strategy;

    private void Awake()
    {
        // Convertimos el componente en una estrategia válida
        strategy = strategyScript as IStrategy;
    }

    // Este método será llamado por el jugador (u otro sistema) para interactuar con el objeto
    public void Interact(GameObject interactor)
    {
        strategy?.Execute(interactor); // Ejecutamos la lógica concreta de la estrategia
    }
}
