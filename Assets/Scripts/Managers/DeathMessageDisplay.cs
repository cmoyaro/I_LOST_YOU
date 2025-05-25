using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// UI que muestra un mensaje de muerte y permite reiniciar la escena
public class DeathMessageDisplay : MonoBehaviour, IObserver
{
    public PlayerDeath playerDeathNotifier;
    public GameObject restartMessage;
    public FirstPersonController playerController;

    private TextMeshProUGUI deathText;
    private bool isDead = false;

    private void Start()
    {
        deathText = GetComponent<TextMeshProUGUI>();

        if (playerDeathNotifier != null)
            playerDeathNotifier.AddObserver(this);

        deathText.enabled = false;

        if (restartMessage != null)
            restartMessage.SetActive(false);
    }

    public void OnNotify()
    {
        deathText.text = "Has muerto";
        deathText.enabled = true;

        if (restartMessage != null)
            restartMessage.SetActive(true);

        if (playerController != null)
            playerController.enabled = false;

        isDead = true;
    }

    private void Update()
    {
        if (isDead && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnDestroy()
    {
        if (playerDeathNotifier != null)
            playerDeathNotifier.RemoveObserver(this);
    }
}
