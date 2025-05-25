using UnityEngine;
using TMPro;

// Detecta objetos interactuables mediante raycast desde la cámara del jugador
public class PlayerInteractor : MonoBehaviour
{
    public float interactDistance = 5f;
    public LayerMask interactLayer;
    public KeyCode interactKey = KeyCode.E;
    public TextMeshProUGUI interactionMessage;
    public GameObject playerObject;

    private InteractableObject currentTarget;

    void Update()
    {
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
                    currentTarget.Interact(playerObject);
                    interactionMessage.gameObject.SetActive(false);
                }

                return;
            }
        }

        currentTarget = null;
        interactionMessage.gameObject.SetActive(false);
    }
}
