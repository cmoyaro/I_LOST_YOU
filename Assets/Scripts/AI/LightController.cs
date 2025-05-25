using UnityEngine;
using System.Collections;

// Controlador para luces individuales: encendido, apagado, cambio de intensidad y efecto de titileo
public class LightController : MonoBehaviour
{
    private Light myLight;
    private Coroutine flickerCoroutine;
    private float baseIntensity;

    void Awake()
    {
        myLight = GetComponent<Light>();
        if (myLight != null)
            baseIntensity = myLight.intensity;
    }

    // Enciende o apaga la luz
    public void SetLight(bool state)
    {
        if (myLight != null)
            myLight.enabled = state;
    }

    // Cambia la intensidad de la luz y guarda ese valor como base
    public void SetIntensity(float intensity)
    {
        if (myLight != null)
        {
            myLight.intensity = intensity;
            baseIntensity = intensity;
        }
    }

    // Inicia el efecto de titileo (tipo vela)
    public void StartFlicker(float flickerSpeed = 0.05f, float flickerAmount = 0.2f)
    {
        if (flickerCoroutine == null)
            flickerCoroutine = StartCoroutine(Flicker(flickerSpeed, flickerAmount));
    }

    // Detiene el titileo y devuelve la luz a su intensidad base
    public void StopFlicker()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
            myLight.intensity = baseIntensity;
        }
    }

    // Corrutina que aplica pequeñas variaciones a la intensidad, simulando una llama
    private IEnumerator Flicker(float speed, float amount)
    {
        while (true)
        {
            float randomOffset = Random.Range(-amount, amount);
            myLight.intensity = Mathf.Clamp(baseIntensity + randomOffset, 0f, baseIntensity + amount);
            yield return new WaitForSeconds(speed);
        }
    }
}
