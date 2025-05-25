using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float crouchSpeed = 2f;
    public float mouseSensitivity = 2f;
    public Transform playerCamera;

    [Header("Agacharse - Cámara")]
    public float crouchCameraHeight = 1.0f;
    public float crouchLerpSpeed = 5f;

    [Header("Linterna")]
    public KeyCode flashlightKey = KeyCode.F;
    private bool canUseFlashlight = true;

    private GameObject flashlightPlayer;
    private GameObject onVisual;
    private GameObject offVisual;
    private Light flashlightLight;

    [Header("Audio")]
    public AudioSource footstepSource;
    public AudioClip leftFootstepClip;
    public AudioClip rightFootstepClip;
    public float walkStepInterval = 0.5f;
    public float runStepInterval = 0.3f;

    private float stepTimer = 0f;
    private bool isLeftStep = true;

    [Header("Gravedad")]
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;
    public Transform groundCheck;
    [HideInInspector] public bool isInputLocked = false;

    private CharacterController controller;
    private float currentSpeed;
    private bool isCrouching = false;

    private float xRotation = 0f;

    private Vector3 velocity;
    private bool isGrounded;

    private Vector3 originalCenter;
    private float originalHeight;

    private Vector3 standingCamLocalPos;
    private Vector3 crouchingCamLocalPos;
    // Inicializa cámara, gravedad y linterna
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        originalCenter = controller.center;
        originalHeight = controller.height;

        standingCamLocalPos = playerCamera.localPosition;

        crouchingCamLocalPos = new Vector3(
            standingCamLocalPos.x,
            crouchCameraHeight,
            standingCamLocalPos.z
        );
    }
    // Control general del jugador (movimiento, cámara y linterna)
    void Update()
    {
        if (!isInputLocked)
        {
            mouseCamera();
            movement();
        }

        hasGravity();

        if (Input.GetKeyDown(flashlightKey) && flashlightPlayer != null && canUseFlashlight)
        {
            toogleFlashlight();
        }
    }
    // Asigna el objeto linterna al recogerlo
    public void SetFlashlight(GameObject flashlightObject)
    {
        flashlightPlayer = flashlightObject;
        onVisual = flashlightPlayer.transform.Find("ON")?.gameObject;
        offVisual = flashlightPlayer.transform.Find("OFF")?.gameObject;
        flashlightLight = onVisual.GetComponentInChildren<Light>();

        if (flashlightLight != null)
            flashlightLight.enabled = false;

        if (onVisual != null) onVisual.SetActive(false);
        if (offVisual != null) offVisual.SetActive(true);
    }

    public void toogleFlashlight()
    {
        bool isOn = flashlightLight.enabled;
        flashlightLight.enabled = !isOn;

        if (onVisual != null) onVisual.SetActive(!isOn);
        if (offVisual != null) offVisual.SetActive(isOn);
    }
    // Control de cámara con ratón
    public void mouseCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void hasGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    // Movimiento, sonido de pasos y agacharse
    public void movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftControl))
            currentSpeed = crouchSpeed;
        else if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed = runSpeed;
        else
            currentSpeed = walkSpeed;

        controller.Move(move * currentSpeed * Time.deltaTime);

        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentStepInterval = isRunning ? runStepInterval : walkStepInterval;

        if (isMoving)
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
                        isLeftStep = !isLeftStep;
                    }
                }
                stepTimer = currentStepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            float heightDiff = originalHeight - 1f;
            controller.height = 1f;
            controller.center = new Vector3(0, 0.5f, 0);
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

        Vector3 targetCamPos = isCrouching ? crouchingCamLocalPos : standingCamLocalPos;
        playerCamera.localPosition = Vector3.Lerp(
            playerCamera.localPosition,
            targetCamPos,
            Time.deltaTime * crouchLerpSpeed
        );
    }

    public bool IsFlashlightOn()
    {
        return flashlightLight != null && flashlightLight.enabled;
    }

    public void TurnOffFlashlight()
    {
        if (flashlightLight != null)
            flashlightLight.enabled = false;

        if (onVisual != null) onVisual.SetActive(false);
        if (offVisual != null) offVisual.SetActive(true);
    }
    // Apaga la linterna tras un parpadeo forzado (efecto paranormal)
    public void ForceFlashlightShutdown(float duration = 1.5f, float blinkInterval = 0.1f)
    {
        if (!IsFlashlightOn()) return;

        StartCoroutine(BlinkThenOff(duration, blinkInterval));
    }

    private IEnumerator BlinkThenOff(float duration, float interval)
    {
        if (flashlightLight == null) yield break;

        float timer = 0f;
        bool originalState = flashlightLight.enabled;

        while (timer < duration)
        {
            bool state = flashlightLight.enabled;
            flashlightLight.enabled = !state;

            if (onVisual != null) onVisual.SetActive(!state);
            if (offVisual != null) offVisual.SetActive(state);

            yield return new WaitForSeconds(interval);
            timer += interval;
        }

        flashlightLight.enabled = originalState;
        if (onVisual != null) onVisual.SetActive(originalState);
        if (offVisual != null) offVisual.SetActive(!originalState);
    }

    private IEnumerator FlashlightCooldown(float seconds)
    {
        canUseFlashlight = false;
        yield return new WaitForSeconds(seconds);
        canUseFlashlight = true;
    }

    public void DisableFlashlight()
    {
        canUseFlashlight = false;
    }

    public void EnableFlashlight()
    {
        canUseFlashlight = true;
    }

    public bool HasFlashlight()
    {
        return flashlightPlayer != null;
    }
}
