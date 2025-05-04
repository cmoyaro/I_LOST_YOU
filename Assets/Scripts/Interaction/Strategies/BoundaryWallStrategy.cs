using UnityEngine;
using System.Collections;

public class BoundaryWallStrategy : MonoBehaviour, IStrategy
{
    [TextArea]
    public string customText = "";  // Mensaje configurable
    public float messageDuration = 3f;       // Tiempo visible antes de desaparecer automáticamente
    public float pushBackDistance = 5f;      // Distancia para empujar hacia atrás después
    public Transform bunkerTarget;           // Referencia al objeto Bunker

    private GameObject player;
    private bool isShowing = false;

    public void Execute(GameObject interactor)
    {
        isShowing = true;

        // Pausamos el juego
        Time.timeScale = 0f;

        player = interactor;

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

        if (player != null && bunkerTarget != null)
        {
            Vector3 directionToBunker = (bunkerTarget.position - player.transform.position).normalized;
            directionToBunker.y = 0;

            if (directionToBunker != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToBunker);
                player.transform.rotation = targetRotation;
            }

            player.transform.position += directionToBunker * pushBackDistance;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<InteractableObject>()?.Interact(other.gameObject);
        }
    }

    // Mostrar el mensaje en pantalla
    private void OnGUI()
    {
        if (isShowing)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 40;
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleCenter;

            // Dibuja el texto en el centro superior de la pantalla
            GUI.Label(new Rect(Screen.width / 2 - 400, Screen.height / 2, 800, 200), customText, style);



        }
    }
}
