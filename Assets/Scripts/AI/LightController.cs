using UnityEngine;
using System.Collections;


// Este script se coloca en cada luz que queramos controlar.
// Permite encender, apagar, cambiar intensidad y hacer titileo suave.
public class LightController : MonoBehaviour
{
    private Light myLight;
    private Coroutine flickerCoroutine;
    private float baseIntensity;

    void Awake()
    {
        myLight = GetComponent<Light>();
        if (myLight == null)
        {
            Debug.LogWarning("No se encontró componente Light en " + gameObject.name);
        }
        else
        {
            baseIntensity = myLight.intensity; // Guardamos la intensidad original
        }
    }
    // Enciende o apaga la luz

    public void SetLight(bool state)
    {
        if (myLight != null)
            myLight.enabled = state;
    }

    // Cambia la intensidad de la luz
    public void SetIntensity(float intensity)
    {
        if (myLight != null)
        {
            myLight.intensity = intensity;
            baseIntensity = intensity;
        }
    }

    // Inicia el efecto titileo tipo vela
    public void StartFlicker(float flickerSpeed = 0.05f, float flickerAmount = 0.2f)
    {
        if (flickerCoroutine == null)
            flickerCoroutine = StartCoroutine(Flicker(flickerSpeed, flickerAmount));
    }

    // Detiene el titileo y deja la luz estable
    public void StopFlicker()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
            myLight.intensity = baseIntensity;
        }
    }

    // Efecto de titileo suave, simulando llama de vela
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
