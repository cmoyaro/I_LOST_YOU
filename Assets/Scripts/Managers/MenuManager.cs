using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Gestiona el menú principal: iniciar juego y salir
public class MenuManager : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public string sceneToLoad = "IntroScene";

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
