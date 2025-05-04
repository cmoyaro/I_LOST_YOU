using UnityEngine;
using System.Collections; 


// Este script se coloca en cada luz que queramos controlar.
// Permite encender, apagar, cambiar intensidad y hacer parpadeos.
public class LightController : MonoBehaviour
{
    private Light myLight;
    private Coroutine flickerCoroutine;

    void Awake()
    {
        myLight = GetComponent<Light>();
        if (myLight == null)
        {
            Debug.LogWarning("No se encontró componente Light en " + gameObject.name);
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
            myLight.intensity = intensity;
    }

    // Hace parpadear la luz (hasta que se llame StopFlicker)
    public void StartFlicker(float flickerSpeed = 0.1f)
    {
        if (flickerCoroutine == null)
            flickerCoroutine = StartCoroutine(Flicker(flickerSpeed));
    }

    // Detiene el parpadeo y deja la luz encendida
    public void StopFlicker()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
            SetLight(true);
        }
    }

    private IEnumerator Flicker(float flickerSpeed)
    {
        while (true)
        {
            SetLight(!myLight.enabled);
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}
