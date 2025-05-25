using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Controla la escena de introducción: muestra texto inicial y permite continuar al pulsar una tecla
public class IntroSceneManager : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public TextMeshProUGUI continueMessage;
    public KeyCode continueKey = KeyCode.Space;
    public string nextSceneName = "Exterior";
    public float delayToShowMessage = 3f;

    private void Start()
    {
        if (introText != null)
            introText.gameObject.SetActive(true);

        if (continueMessage != null)
            continueMessage.gameObject.SetActive(false);

        Invoke(nameof(ShowContinueMessage), delayToShowMessage);
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
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
