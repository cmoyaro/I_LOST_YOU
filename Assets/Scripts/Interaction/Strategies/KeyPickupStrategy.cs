using UnityEngine;
using System.Collections;

// Estrategia para recoger la llave y mostrar un mensaje temporal en pantalla usando OnGUI (HUD rápido).
// Lo activamos desde InteractableObject como siempre con el patrón Strategy.
public class KeyPickupStrategy : MonoBehaviour, IStrategy
{
    public KeyData keyData;                        // Aquí asignamos la llave concreta que recoge el jugador (ScriptableObject)
    [TextArea]
    public string pickupMessage = "Has recogido una llave";  // Mensaje que queremos que aparezca cuando recogemos la llave
    public float messageDuration = 2f;             // Cuánto tiempo dejamos ese mensaje visible en pantalla

    private bool pickedUp = false;
    private bool isShowing = false;                // Para controlar si estamos mostrando el mensaje ahora mismo

    public void Execute(GameObject interactor)
    {
        if (pickedUp) return;                      // Si ya la recogimos, ignoramos nuevas interacciones
        pickedUp = true;

        // Buscamos el inventario del jugador para añadir la llave recogida
        var inventory = interactor.GetComponent<PlayerInventory>();
        if (inventory != null && keyData != null)
        {
            inventory.AddKey(keyData);
        }
        else
        {
            Debug.LogWarning("No se encontró PlayerInventory o falta asignar KeyData en el inspector.");
        }

        // Activamos el mensaje para que se vea en pantalla
        isShowing = true;
        StartCoroutine(HideMessageAfterDelay());

        // Además ocultamos el objeto y lo destruimos después para limpiar la escena
        StartCoroutine(HideAndDestroy());
    }

    // Esta corrutina simplemente oculta el mensaje tras unos segundos
    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        isShowing = false;
    }

    // Aquí ocultamos la parte visual (cilindros, colliders...) y lo eliminamos después
    private IEnumerator HideAndDestroy()
    {
        foreach (var r in GetComponentsInChildren<MeshRenderer>())
            r.enabled = false;
        foreach (var c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        yield return new WaitForSeconds(messageDuration);
        Destroy(gameObject);
    }

    // Mostramos el mensaje en pantalla usando OnGUI
    private void OnGUI()
    {
        if (isShowing)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 40;
            style.normal.textColor = Color.green;      
            style.alignment = TextAnchor.MiddleCenter;

            // Dibujamos el texto en el centro de la pantalla
            GUI.Label(
                new Rect(Screen.width / 2 - 400, Screen.height / 2, 800, 200),
                pickupMessage,
                style
            );
        }
    }
}
