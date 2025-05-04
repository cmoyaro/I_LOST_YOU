using UnityEngine;
using System.Collections;

// Estrategia concreta para recoger una linterna.
// Este script implementa la interfaz IStrategy y define qué ocurre cuando se recoge este objeto.
// Se activa desde InteractableObject, aplicando el patrón Strategy para separar lógica y comportamiento.
public class FlashlightPickupStrategy : MonoBehaviour, IStrategy
{
    public GameObject playerFlashlight;   // La linterna del jugador que se activa al recogerla
    public GameObject messageUI;           // Mensaje "Pulsa F para encender/apagar"
    public float messageDuration = 5f;     // Tiempo que se muestra el mensaje

    private bool pickedUp = false;

    public void Execute(GameObject interactor)
{
    if (pickedUp) return;
    pickedUp = true;

    Debug.Log("FlashlightPickup: Ejecutando... Interactor = " + interactor.name);

    // Activamos la linterna oculta que el jugador ya tiene
    if (playerFlashlight != null)
    {
        playerFlashlight.SetActive(true);
    }

    // Pasamos la referencia al FirstPersonController para que sepa cuál es la linterna
    FirstPersonController fpc = interactor.GetComponent<FirstPersonController>();
    if (fpc != null && playerFlashlight != null)
    {
        Debug.Log("FlashlightPickup: FirstPersonController encontrado, llamando SetFlashlight...");
        fpc.SetFlashlight(playerFlashlight);
    }
    else
    {
        Debug.Log("FlashlightPickup: NO se encontró FirstPersonController en " + interactor.name);
    }

    StartCoroutine(ShowMessageAndDestroy());
}


    // Oculta TODO lo visual y colisiones del objeto y sus hijos
    private void HideWorldObject()
    {   
         // Oculta todo el MeshRenderer (padre + hijos)
        foreach (var r in GetComponentsInChildren<MeshRenderer>())
        {
            r.enabled = false;
        }
         // Desactiva todas las colisiones (padre + hijos)
        foreach (var c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
        //Apaga TODAS las luces que haya (padre + hijos)
        foreach (var light in GetComponentsInChildren<Light>())
        {
            light.enabled = false;
        }
    }

    private IEnumerator ShowMessageAndDestroy()
    {
        // Mostramos el mensaje en pantalla
        if (messageUI != null)
            messageUI.SetActive(true);

        // Ocultamos el objeto entero (linterna + hijos)
        HideWorldObject();

        yield return new WaitForSeconds(messageDuration);

        if (messageUI != null)
            messageUI.SetActive(false);

        Destroy(gameObject);  // Finalmente eliminamos el objeto de la mesa
    }
}
