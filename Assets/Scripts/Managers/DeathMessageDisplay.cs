using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necesario para reiniciar la escena
using TMPro;

// Este script va en un objeto de la UI y se encarga de mostrar el mensaje "Has muerto o Has fracasado, etc..." cuando el jugador muere
public class DeathMessageDisplay : MonoBehaviour, IObserver
{
    public PlayerDeath playerDeathNotifier; // Referencia al script que notifica la muerte (va en el jugador)
    private TextMeshProUGUI deathText; // privado y automático

    // referencia al texto de "Pulsa R para reiniciar", lo asigno desde el inspector
    public GameObject restartMessage;

    // referencia al controlador del jugador para desactivarlo al morir
    public FirstPersonController playerController;

    private bool isDead = false; // Nuevo: para controlar si ya hemos muerto

    private void Start()
    {
        // Obtenemos el componente textual que está en el mismo objeto
        deathText = GetComponent<TextMeshProUGUI>();

        if (playerDeathNotifier != null)
        {
            Debug.Log("Registrando observador...");
            playerDeathNotifier.AddObserver(this);
        }
        else
        {
            Debug.LogWarning("playerDeathNotifier es null en DeathMessageDisplay.");
        }

        // Ocultamos el texto al empezar
        deathText.enabled = false;

        // Ocultamos también el mensaje de reinicio al empezar
        if (restartMessage != null)
        {
            restartMessage.SetActive(false);
        }
    }

    // Este método se ejecuta automáticamente cuando el jugador muere
    public void OnNotify()
    {
        Debug.Log("Se recibió notificación de muerte.");
        deathText.text = "Has muerto"; //  En el futuro, he de cambiar el texto por algo más imaginativo...
        deathText.enabled = true; // Lo activa en pantalla

        // Mostramos el mensaje de reinicio
        if (restartMessage != null)
        {
            restartMessage.SetActive(true);
        }

        // Desactivamos el controlador del jugador para que no se pueda mover
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Marcamos que el jugador está muerto para permitir reinicio con tecla R
        isDead = true;
    }

    private void Update()
    {
        //Si estamos muertos y pulsamos R, reiniciamos la escena
        if (isDead && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnDestroy()
    {
        playerDeathNotifier.RemoveObserver(this); // Me borro como observador si este objeto se destruye
    }
}
