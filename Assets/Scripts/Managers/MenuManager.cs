using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Script sencillo para gestionar el menú principal.
// Se encarga de manejar los botones: Empezar y Salir.
// Lo uso para enlazarlo desde la UI directamente.

public class MenuManager : MonoBehaviour
{
    public Button startButton;  // Botón para empezar el juego
    public Button quitButton;   // Botón para cerrar el juego
    public string sceneToLoad = "IntroScene";  // Nombre de la escena a cargar al empezar

    void Start()
    {
        // Mostrar y desbloquear el cursor al llegar al menú
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // Asignamos las funciones a los botones
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    // Carga la escena de inicio del juego
    void StartGame()
    {
        Debug.Log("Iniciando el juego...");
        SceneManager.LoadScene(sceneToLoad);
    }

    // Cierra la aplicación
    void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

#if UNITY_EDITOR
        // Esto es solo para pruebas en el editor de Unity
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
