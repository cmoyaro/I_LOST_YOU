using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// Estrategia para finalizar el juego al interactuar con el perro (u objeto final)
public class GameWinStrategy : MonoBehaviour, IStrategy
{
    [TextArea] public string winMessage = "¡Has encontrado a Luna! Pero todavía queda salir...";
    public float delayBeforeReturn = 3f;

    private bool hasWon = false;
    private bool isShowing = false;

    public void Execute(GameObject interactor)
    {
        if (hasWon) return;

        hasWon = true;
        isShowing = true;

        StartCoroutine(WinAndLoadMenu());
    }

    private IEnumerator WinAndLoadMenu()
    {
        yield return new WaitForSeconds(delayBeforeReturn);
        SceneManager.LoadScene("Menu");
    }

    private void OnGUI()
    {
        if (!isShowing) return;

        // Fondo oscuro
        Color prevColor = GUI.color;
        GUI.color = new Color(0, 0, 0, 0.75f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = prevColor;

        // Mensaje de victoria
        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 40,
            normal = { textColor = Color.blue },
            alignment = TextAnchor.MiddleCenter
        };

        GUI.Label(
            new Rect(Screen.width / 2 - 400, Screen.height / 2 - 100, 800, 200),
            winMessage,
            style
        );
    }
}
