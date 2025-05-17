using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIDirector : MonoBehaviour
{
    [Header("Gestión de luces")]
    public List<LightController> lights = new List<LightController>(); // Arrastra aquí todas las luces

    [Header("Parámetros de decisión")]
    public float decisionInterval = 5f;     // Cada cuánto tiempo toma decisiones
    public float flickerChance = 0.3f;      // Probabilidad de que una luz empiece a titilar
    public float turnOffChance = 0.2f;      // Probabilidad de que una luz se apague
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;

    [Header("Parámetros del titileo")]
    public float flickerSpeedMin = 0.05f;   // Velocidad mínima del titileo
    public float flickerSpeedMax = 0.08f;    // Velocidad máxima del titileo
    public float flickerAmountMin = 0.05f;  // Variación mínima de intensidad
    public float flickerAmountMax = 0.12f;  // Variación máxima de intensidad


    private float timer = 0f;
    [Header("Evento paranormal: control de activación")]
    private float lastParanormalEventTime = -Mathf.Infinity;
    public float paranormalCooldown = 20f; // Tiempo mínimo entre eventos (en segundos)
    private bool isParanormalEventActive = false;



    [Header("Jugador")]
    public GameObject player;
    [Header("Audio paranormal")]
    public AudioSource ambientAudioSource;      // Fuente de sonido para eventos sobrenaturales
    public AudioClip breathInClip;              // Clip de sonido de aspiración (lo arrastras tú)



    void Start()
    {
        foreach (var light in lights)
        {
            if (light == null) continue;
            light.SetLight(true);
            float initialIntensity = Random.Range(minIntensity, maxIntensity);
            light.SetIntensity(initialIntensity);
            light.StopFlicker(); // Por si estaba en alguna rutina vieja
        }
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= decisionInterval)
        {
            MakeDecision();
            timer = 0f;
        }
    }
    // Este método hace que el AI Director tome decisiones "dinámicas"
    private void MakeDecision()
    {
        Debug.Log("AI Director está tomando decisiones...");

        foreach (var light in lights)
        {
            float randomValue = Random.value;

            if (randomValue < flickerChance)
            {
                // Activamos un titileo suave estilo vela
                float speed = Random.Range(flickerSpeedMin, flickerSpeedMax);
                float amount = Random.Range(flickerAmountMin, flickerAmountMax);
                light.SetLight(true);
                light.StartFlicker(speed, amount);
            }
            else if (randomValue < flickerChance + turnOffChance)
            {
                // Apagamos la luz por completo
                light.StopFlicker();
                light.SetLight(false);
            }
            else
            {
                // Luz estable con intensidad aleatoria
                light.StopFlicker();
                light.SetLight(true);
                float newIntensity = Random.Range(minIntensity, maxIntensity);
                light.SetIntensity(newIntensity);
            }
        }

        // Evento paranormal aleatorio con cooldown y bloqueo activo
        if (Time.time - lastParanormalEventTime >= paranormalCooldown)
        {
            if (!isParanormalEventActive && Random.value < 0.05f)
            {
                Debug.Log("AI Director: ACTIVANDO evento paranormal.");
                ForceFlashlightGlitchAndAffectArea(8f, 1.8f, 0.07f);
                lastParanormalEventTime = Time.time;
            }
            else
            {
                Debug.Log("AI Director: Evento paranormal NO activado esta vez.");
            }
        }
        else
        {
            Debug.Log("AI Director: Evento paranormal en cooldown.");
        }
    }

    // Llama al jugador y le apaga la linterna con parpadeo
    public void ForceFlashlightGlitchAndAffectArea(float radius = 10f, float duration = 1.5f, float blinkInterval = 0.1f)
    {

        if (isParanormalEventActive)
        {
            Debug.Log("AI Director: Evento paranormal ya activo, ignorando nueva llamada.");
            return;
        }

        isParanormalEventActive = true;


        if (player == null) return;

        // 1. Apagar la linterna con parpadeo
        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        if (fpc == null)
        {
            Debug.LogWarning("AI Director: No se encontró FirstPersonController en el objeto del jugador.");
            return;
        }

       
        // Si la linterna está encendida, la apagamos con parpadeo
        if (fpc.IsFlashlightOn())
        {
            Debug.Log("AI Director: Apagando linterna del jugador con efecto parpadeo.");
            fpc.ForceFlashlightShutdown(duration, blinkInterval);
        }
        else
        {
            Debug.Log("AI Director: La linterna ya estaba apagada.");
        }

        // Reproducimos sonido si está todo configurado
        if (ambientAudioSource != null && breathInClip != null)
        {
            Debug.Log("AI Director: Reproduciendo sonido de aspiración.");
            ambientAudioSource.PlayOneShot(breathInClip);
        }


        // 2. Afectar luces cercanas al jugador
        Debug.Log("AI Director: Revisando luces cercanas para provocar fallos.");
        foreach (LightController light in lights)
        {
            if (light == null) continue;

            float dist = Vector3.Distance(player.transform.position, light.transform.position);
            if (dist <= radius)
            {
                float rand = Random.value;

                if (rand < 0.5f)
                {
                    // Titileo rápido
                    Debug.Log($"Luz {light.gameObject.name}: inicia titileo rápido.");
                    light.StartFlicker(blinkInterval, 0.15f);
                }
                else
                {
                    // Apagón breve
                    Debug.Log($"Luz {light.gameObject.name}: se apaga temporalmente.");
                    StartCoroutine(TemporaryLightOff(light, duration));
                }
            }
        }
        StartCoroutine(EndParanormalEventAfter(duration + 0.5f));
    }


    // Corrutina para apagar una luz temporalmente
    private IEnumerator TemporaryLightOff(LightController light, float duration)
    {
        light.StopFlicker();
        light.SetLight(false);
        yield return new WaitForSeconds(duration);
        light.SetLight(true);
    }

    private IEnumerator EndParanormalEventAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        isParanormalEventActive = false;
        Debug.Log("AI Director: Evento paranormal finalizado.");
    }

}