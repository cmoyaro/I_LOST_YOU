using UnityEngine;
using System.Collections;

// Estrategia concreta para recoger una linterna.
// Se activa desde InteractableObject, aplicando el patrón Strategy para separar lógica y comportamiento.
public class FlashlightPickupStrategy : MonoBehaviour, IStrategy
{
    public GameObject playerFlashlight;
    public GameObject messageUI;
    public float messageDuration = 5f;

    private bool pickedUp = false;

    public void Execute(GameObject interactor)
    {
        if (pickedUp) return;
        pickedUp = true;

        if (playerFlashlight != null)
            playerFlashlight.SetActive(true);

        FirstPersonController fpc = interactor.GetComponent<FirstPersonController>();
        if (fpc != null && playerFlashlight != null)
            fpc.SetFlashlight(playerFlashlight);

        StartCoroutine(ShowMessageAndDestroy());
    }

    // Oculta los elementos visuales y colisiones del objeto
    private void HideWorldObject()
    {
        foreach (var r in GetComponentsInChildren<MeshRenderer>())
            r.enabled = false;

        foreach (var c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        foreach (var light in GetComponentsInChildren<Light>())
            light.enabled = false;
    }

    private IEnumerator ShowMessageAndDestroy()
    {
        if (messageUI != null)
            messageUI.SetActive(true);

        HideWorldObject();

        yield return new WaitForSeconds(messageDuration);

        if (messageUI != null)
            messageUI.SetActive(false);

        Destroy(gameObject);
    }
}
