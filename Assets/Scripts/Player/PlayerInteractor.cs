using UnityEngine;
using TMPro;

// Este script detecta objetos con los que el jugador puede interactuar.
// Va en la cámara del jugador para lanzar un raycast desde su punto de vista.
// Usa InteractableObject en lugar de una interfaz directa, lo que permite aplicar el patrón Strategy
// y dejar que cada objeto defina su comportamiento sin que el jugador necesite conocer los detalles.
public class PlayerInteractor : MonoBehaviour
{
    public float interactDistance = 5f;                     // Distancia máxima para interactuar
    public LayerMask interactLayer;                         // Capa de objetos interactuables
    public KeyCode interactKey = KeyCode.E;                 // Tecla de interacción
    public TextMeshProUGUI interactionMessage;              // UI que muestra "Pulsa E para..."
    public GameObject playerObject;  // El objeto player


    private InteractableObject currentTarget;               // Objeto que estamos mirando ahora

    void Update()
    {
        // Lanza un rayo hacia adelante para detectar objetos interactuables
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();

            if (interactable != null)
            {
                currentTarget = interactable;
                interactionMessage.gameObject.SetActive(true);

                if (Input.GetKeyDown(interactKey))
                {
                    currentTarget.Interact(playerObject); // Pasamos el jugador como interactor
                    interactionMessage.gameObject.SetActive(false);
                }

                return; // Salimos para no ocultar el mensaje
            }
        }

        // Si no estamos mirando a ningún objeto interactuable
        currentTarget = null;
        interactionMessage.gameObject.SetActive(false);
    }
}
