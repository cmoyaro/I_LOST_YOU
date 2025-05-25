using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// Estrategia para abrir puertas que pueden requerir llave, mostrar mensaje o activar teletransporte
public class DoorInteractionStrategy : MonoBehaviour, IStrategy
{
    public Transform doorPivot;
    public float openAngle = 90f;
    public float openSpeed = 2f;

    [Header("Spawner de objetos (Factory)")]
    public ObjectSpawner objectSpawner;
    private bool hasSpawnedObjects = false;

    [Header("Llave para puerta")]
    public bool requiresKey = false;
    public KeyData requiredKey;
    [TextArea] public string lockedMessage = "La puerta está cerrada. Necesitas la llave.";
    public float messageDuration = 2f;
    private bool isShowing = false;

    [Header("Opcional - Teletransporte")]
    public bool enableTeleport = false;
    public string nextSceneName;
    public float teleportDelay = 2f;

    private bool isOpen = false;
    private bool isAnimating = false;
    private bool hasTeleported = false;

    public void Execute(GameObject interactor)
    {
        if (requiresKey)
        {
            if (requiredKey == null) return;

            var inventory = interactor.GetComponent<PlayerInventory>();
            if (inventory == null) return;

            if (!inventory.HasKey(requiredKey))
            {
                isShowing = true;
                StartCoroutine(HideMessageAfterDelay());
                return;
            }
        }

        if (!isAnimating)
            StartCoroutine(RotateDoor(interactor));
    }

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

        if (objectSpawner != null && !hasSpawnedObjects)
        {
            objectSpawner.TrySpawnObjects();
            hasSpawnedObjects = true;
        }

        if (enableTeleport && !hasTeleported)
        {
            hasTeleported = true;
            yield return new WaitForSeconds(teleportDelay);

            if (!string.IsNullOrEmpty(nextSceneName))
                SceneManager.LoadScene(nextSceneName);
        }
    }

    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        isShowing = false;
    }

    private void OnGUI()
    {
        if (!isShowing) return;

        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 40,
            normal = { textColor = Color.yellow },
            alignment = TextAnchor.MiddleCenter
        };

        GUI.Label(
            new Rect(Screen.width / 2 - 400, Screen.height / 2, 800, 200),
            lockedMessage,
            style
        );
    }
}
