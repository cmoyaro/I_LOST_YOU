using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;  // Para poder teletransportar entre escenas

// Script para abrir puertas (pueden necesitar llave o no) y, opcionalmente, hacer teletransporte.
// Ahora también muestra un mensaje visible para el jugador si le falta la llave (HUD rápido con OnGUI).
public class DoorInteractionStrategy : MonoBehaviour, IStrategy
{
    public Transform doorPivot;             // Punto donde gira la puerta (bisagra)
    public float openAngle = 90f;           // Ángulo de apertura
    public float openSpeed = 2f;            // Velocidad a la que se abre

    [Header("Spawner de objetos (Factory)")]
    public ObjectSpawner objectSpawner;     // Si queremos spawnear gusanos o cualquier cosa cuando abrimos

    private bool hasSpawnedObjects = false;

    [Header("Llave para puerta")]
    public bool requiresKey = false;        // Actívalo si la puerta necesita llave
    public KeyData requiredKey;             // La llave que abre esta puerta
    [TextArea]
    public string lockedMessage = "La puerta está cerrada. Necesitas la llave.";  // Mensaje que mostramos si falta la llave
    public float messageDuration = 2f;      // Tiempo que dejamos visible ese mensaje

    private bool isShowing = false;         // Para controlar si estamos mostrando el mensaje ahora

    [Header("Opcional - Teletransporte")]
    public bool enableTeleport = false;     // Activar si queremos teletransportar
    public string nextSceneName;            // Nombre de la escena a la que saltamos
    public float teleportDelay = 2f;        // Cuánto esperamos antes de teletransportar (para dar tiempo a la animación)

    private bool isOpen = false;
    private bool isAnimating = false;
    private bool hasTeleported = false;

    public void Execute(GameObject interactor)
    {
        Debug.Log("DoorInteraction: Ejecutando acción...");

        // Si requiere llave, comprobamos el inventario del jugador
        if (requiresKey)
        {
            if (requiredKey == null)
            {
                Debug.LogWarning("La puerta está configurada para requerir llave pero no se ha asignado ningún KeyData.");
                return;
            }

            var inventory = interactor.GetComponent<PlayerInventory>();
            if (inventory == null)
            {
                Debug.LogError("No se encontró PlayerInventory en: " + interactor.name);
                return;
            }

            // Si NO tenemos la llave => mostramos mensaje informativo al jugador
            if (!inventory.HasKey(requiredKey))
            {
                Debug.Log("La puerta está cerrada: falta la llave.");
                isShowing = true;
                StartCoroutine(HideMessageAfterDelay());
                return;
            }
        }

        // Si llegamos aquí es porque no necesita llave o la tenemos ✅
        if (!isAnimating)
            StartCoroutine(RotateDoor(interactor));
    }

    // Animamos la apertura (o cierre) de la puerta. Esto esta por mejorar, es bastante cutre.
    private IEnumerator RotateDoor(GameObject interactor)
    {
        isAnimating = true;

        Quaternion startRot = doorPivot.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, isOpen ? openAngle : -openAngle, 0);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            doorPivot.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        isOpen = !isOpen;
        isAnimating = false;

        // Si hay un spawner y aún no hemos spawneado nada, lo lanzamos ahora
        if (objectSpawner != null && !hasSpawnedObjects)
        {
            objectSpawner.TrySpawnObjects();
            hasSpawnedObjects = true;
        }

        // Teletransporte opcional (solo si está activado)
        if (enableTeleport && !hasTeleported)
        {
            hasTeleported = true;
            Debug.Log("Activando teletransporte...");

            yield return new WaitForSeconds(teleportDelay);

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("No se ha definido el nombre de la escena de destino.");
            }
        }
    }

    // Oculta el mensaje después de unos segundos
    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        isShowing = false;
    }

    // Dibujamos el mensaje en pantalla con OnGUI (HUD rápido)
    private void OnGUI()
    {
        if (isShowing)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 40;
            style.normal.textColor = Color.yellow;  // Amarillo para que destaque
            style.alignment = TextAnchor.MiddleCenter;

            GUI.Label(
                new Rect(Screen.width / 2 - 400, Screen.height / 2, 800, 200),
                lockedMessage,
                style
            );
        }
    }
}
