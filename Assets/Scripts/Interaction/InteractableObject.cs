using UnityEngine;

// Objeto interactuable que delega su comportamiento en una estrategia externa (patrón Strategy)
public class InteractableObject : MonoBehaviour
{
    [Tooltip("Script que define qué ocurre al interactuar. Debe implementar IStrategy.")]
    public MonoBehaviour strategyScript;

    private IStrategy strategy;

    private void Awake()
    {
        strategy = strategyScript as IStrategy;
    }

    // Llamado desde el jugador para ejecutar la acción definida por la estrategia
    public void Interact(GameObject interactor)
    {
        strategy?.Execute(interactor);
    }
}
