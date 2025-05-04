using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroSceneManager : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public TextMeshProUGUI continueMessage;
    public KeyCode continueKey = KeyCode.Space;
    public string nextSceneName = "Exterior";
    public float delayToShowMessage = 3f; // segundos

    private void Start()
    {
        if (introText != null)
            introText.gameObject.SetActive(true);

        if (continueMessage != null)
            continueMessage.gameObject.SetActive(false); // Ocultar al principio

        Invoke("ShowContinueMessage", delayToShowMessage);
    }

    private void ShowContinueMessage()
    {
        if (continueMessage != null)
            continueMessage.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(continueKey))
        {
            Debug.Log("Cargando la escena: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
