using UnityEngine;
using System.Collections;

// Estrategia que bloquea al jugador, muestra un mensaje y lo redirige hacia el búnker
public class BoundaryWallStrategy : MonoBehaviour, IStrategy
{
    [TextArea] public string customText = "";
    public float messageDuration = 3f;
    public float pushBackDistance = 5f;
    public Transform bunkerTarget;

    private GameObject player;
    private bool isShowing = false;

    public void Execute(GameObject interactor)
    {
        isShowing = true;
        Time.timeScale = 0f;

        player = interactor;
        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        if (fpc != null)
            fpc.isInputLocked = true;

        StartCoroutine(WaitAndExecute());
    }

    private IEnumerator WaitAndExecute()
    {
        float elapsed = 0f;
        while (elapsed < messageDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        HideMessageAndRotateAndPushBack();
    }

    private void HideMessageAndRotateAndPushBack()
    {
        isShowing = false;
        Time.timeScale = 1f;

        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        if (fpc != null)
            fpc.isInputLocked = false;

        if (player != null && bunkerTarget != null)
        {
            Vector3 direction = (bunkerTarget.position - player.transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
                player.transform.rotation = Quaternion.LookRotation(direction);

            player.transform.position += direction * pushBackDistance;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GetComponent<InteractableObject>()?.Interact(other.gameObject);
    }

    private void OnGUI()
    {
        if (!isShowing) return;

        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 40,
            normal = { textColor = Color.blue },
            alignment = TextAnchor.MiddleCenter
        };

        GUI.Label(new Rect(Screen.width / 2 - 400, Screen.height / 2, 800, 200), customText, style);
    }
}
