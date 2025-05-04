using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script para controlar al jugador en primera persona
// Incluye movimiento, rotación con el ratón, agacharse, gravedad y linterna
public class FirstPersonController : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 4f;            // Velocidad normal
    public float runSpeed = 7f;             // Velocidad al correr
    public float crouchSpeed = 2f;          // Velocidad agachado
    public float mouseSensitivity = 2f;     // Sensibilidad del ratón
    public Transform playerCamera;          // Referencia a la cámara

    [Header("Agacharse - Cámara")]
    public float crouchCameraHeight = 1.0f; // Altura de la cámara al agacharse
    public float crouchLerpSpeed = 5f;      // Suavizado del cambio de altura de cámara

    [Header("Linterna")]
    public KeyCode flashlightKey = KeyCode.F;  // Tecla para encender/apagar

    private GameObject flashlightPlayer;       // El objeto raíz (FlashLightPlayer)
    private GameObject onVisual;               // El hijo ON (lo que se ve encendido)
    private GameObject offVisual;              // El hijo OFF (lo que se ve apagado)
    private Light flashlightLight;             // La luz real (Spot Light)

    [Header("Audio")]
    public AudioSource footstepSource;          // Fuente para reproducir pasos
    public AudioClip leftFootstepClip;          // Sonido para pie izquierdo
    public AudioClip rightFootstepClip;         // Sonido para pie derecho
    public float walkStepInterval = 0.5f;       // Intervalo andando normal
    public float runStepInterval = 0.3f;        // Intervalo corriendo

    private float stepTimer = 0f;               // Temporizador entre pasos
    private bool isLeftStep = true;             // Alternar entre pie izquierdo y derecho


    [Header("Gravedad")] //Este campo por el momento no está terminado
    public float gravity = -9.81f;               // Valor de la gravedad
    public float groundCheckDistance = 0.4f;     // Distancia del check de suelo
    public LayerMask groundMask;                 // Qué se considera suelo
    public Transform groundCheck;                // Objeto que marca desde dónde se hace el check

    // Componentes y controladores internos
    private CharacterController controller;      // Componente que gestiona colisiones y movimiento
    private float currentSpeed;                  // Velocidad actual (según estado)
    private bool isCrouching = false;            // ¿Está agachado?

    // Ratón y rotación
    private float xRotation = 0f;

    // Gravedad y suelo
    private Vector3 velocity;                    // Velocidad en el eje Y
    private bool isGrounded;                     // ¿Está tocando suelo?

    // Valores originales del CharacterController
    private Vector3 originalCenter;
    private float originalHeight;

    // Posiciones de cámara en estado normal y agachado
    private Vector3 standingCamLocalPos;
    private Vector3 crouchingCamLocalPos;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Oculta y bloquea el cursor en pantalla

        originalCenter = controller.center;
        originalHeight = controller.height;

        // Guardamos posición de cámara de pie
        standingCamLocalPos = playerCamera.localPosition;

        // Calculamos la posición que tendrá la cámara agachada
        crouchingCamLocalPos = new Vector3(
            standingCamLocalPos.x,
            crouchCameraHeight,
            standingCamLocalPos.z
        );
    }

    void Update()
    {
        // -------------------------
        // Rotación con el ratón
        // -------------------------
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Para no girar la cabeza del revés, es decir, hacer el Reagan
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX); // Rotamos el cuerpo del jugador

        // -------------------------
        // Movimiento con teclado
        // -------------------------
        float x = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S
        Vector3 move = transform.right * x + transform.forward * z;

        // Velocidad según si va andando, corriendo o agachado
        if (Input.GetKey(KeyCode.LeftControl))
            currentSpeed = crouchSpeed;
        else if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed = runSpeed;
        else
            currentSpeed = walkSpeed;

        // Aplicamos el movimiento
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Sonido de pasos 
        // -------------------------
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentStepInterval = isRunning ? runStepInterval : walkStepInterval;

        if (isGrounded && isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                if (footstepSource != null)
                {
                    AudioClip clipToPlay = isLeftStep ? leftFootstepClip : rightFootstepClip;
                    if (clipToPlay != null)
                    {
                        footstepSource.PlayOneShot(clipToPlay);
                        isLeftStep = !isLeftStep;  // Alternamos pie
                    }
                }
                stepTimer = currentStepInterval;
            }
        }
        else
        {
            // Reiniciamos el temporizador cuando dejamos de andar
            stepTimer = 0f;
        }


        // -------------------------
        // Agacharse
        // -------------------------
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // Ajustamos la altura y el centro del CharacterController
            float heightDiff = originalHeight - 1f;
            controller.height = 1f;
            controller.center = new Vector3(0, 0.5f, 0);

            // Ajustamos la posición del jugador para que no flote
            transform.position -= new Vector3(0, heightDiff / 2f, 0);
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            float heightDiff = 2f - controller.height;
            controller.height = originalHeight;
            controller.center = originalCenter;
            transform.position += new Vector3(0, heightDiff / 2f, 0);
            isCrouching = false;
        }

        // -------------------------
        // Suavizado de cámara al agacharse
        // -------------------------
        Vector3 targetCamPos = isCrouching ? crouchingCamLocalPos : standingCamLocalPos;
        playerCamera.localPosition = Vector3.Lerp(
            playerCamera.localPosition,
            targetCamPos,
            Time.deltaTime * crouchLerpSpeed
        );

        // -------------------------
        // Linterna
        // -------------------------
        if (Input.GetKeyDown(flashlightKey) && flashlightPlayer != null)
        {
            Debug.Log("Pulsada F. Linterna activa: " + flashlightLight.enabled);

            bool isOn = flashlightLight.enabled;

            flashlightLight.enabled = !isOn;

            if (onVisual != null) onVisual.SetActive(!isOn);
            if (offVisual != null) offVisual.SetActive(isOn);
        }
        // -------------------------
        // Detección de suelo
        // -------------------------
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            // Pequeño empuje hacia abajo para que no flote
            velocity.y = -2f;
        }

        // -------------------------
        // Aplicar gravedad
        // -------------------------
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    // Método para asignar la linterna recogida
    public void SetFlashlight(GameObject flashlightObject)
    {
        flashlightPlayer = flashlightObject;
        onVisual = flashlightPlayer.transform.Find("ON")?.gameObject;
        offVisual = flashlightPlayer.transform.Find("OFF")?.gameObject;
        flashlightLight = onVisual.GetComponentInChildren<Light>();

        Debug.Log("SetFlashlight llamado:");
        Debug.Log("  ON: " + (onVisual != null));
        Debug.Log("  OFF: " + (offVisual != null));
        Debug.Log("  Light encontrado: " + (flashlightLight != null));

        // Desactivar la luz al recogerla
        if (flashlightLight != null)
        {
            flashlightLight.enabled = false;
        }

        // Activar/desactivar visuals para que empiece apagada
        if (onVisual != null) onVisual.SetActive(false);
        if (offVisual != null) offVisual.SetActive(true);
    }



}
