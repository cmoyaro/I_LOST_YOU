using UnityEngine;
using System.Collections;

// Estrategia para recoger una llave y mostrar un mensaje temporal en pantalla
public class KeyPickupStrategy : MonoBehaviour, IStrategy
{
    public KeyData keyData;
    [TextArea] public string pickupMessage = "Has recogido una llave";
    public float messageDuration = 2f;

    private bool pickedUp = false;
    private bool isShowing = false;

    public void Execute(GameObject interactor)
    {
        if (pickedUp) return;
        pickedUp = true;

        var inventory = interactor.GetComponent<PlayerInventory>();
        if (inventory != null && keyData != null)
            inventory.AddKey(keyData);

        isShowing = true;
        StartCoroutine(HideMessageAfterDelay());
        StartCoroutine(HideAndDestroy());
    }

    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        isShowing = false;
    }

    private IEnumerator HideAndDestroy()
    {
        foreach (var r in GetComponentsInChildren<MeshRenderer>())
            r.enabled = false;

        foreach (var c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        yield return new WaitForSeconds(messageDuration);
        Destroy(gameObject);
    }

    private void OnGUI()
    {
        if (!isShowing) return;

        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 40,
            normal = { textColor = Color.green },
            alignment = TextAnchor.MiddleCenter
        };

        GUI.Label(
            new Rect(Screen.width / 2 - 400, Screen.height / 2, 800, 200),
            pickupMessage,
            style
        );
    }
}
