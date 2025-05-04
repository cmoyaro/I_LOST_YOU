using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// Estrategia para finalizar el juego al interactuar con el perro (u objeto final)
public class GameWinStrategy : MonoBehaviour, IStrategy
{
    [TextArea]
    public string winMessage = "¡Has encontrado a Luna! Pero todavía queda salir...";

    public float delayBeforeReturn = 3f;   // Tiempo antes de volver al menú

    private bool hasWon = false;
    private bool isShowing = false;        // Si mostramos el mensaje

    private void Awake()
    {
        // Por si acaso queremos inicializar algo más adelante
    }

    public void Execute(GameObject interactor)
    {
        if (hasWon) return;
        hasWon = true;

        Debug.Log("¡Victoria alcanzada!");

        // Mostramos el mensaje en pantalla
        isShowing = true;

        // Llamamos a la corrutina para ir al menú
        StartCoroutine(WinAndLoadMenu());
    }

    private IEnumerator WinAndLoadMenu()
    {
        yield return new WaitForSeconds(delayBeforeReturn);

        Debug.Log("Cargando escena 'Menu'...");
        SceneManager.LoadScene("Menu");
    }

    private void OnGUI()
    {
        if (isShowing)
        {
            // Dibujar fondo oscuro
            Color prevColor = GUI.color;
            GUI.color = new Color(0, 0, 0, 0.75f);  // Negro con 75% opacidad
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = prevColor;

            // Mostrar mensaje en en el centro
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 40;
            style.normal.textColor = Color.blue;
            style.alignment = TextAnchor.MiddleCenter;

            GUI.Label(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 100, 800, 200), winMessage, style);
        }
    }
}
