using UnityEngine;
using System.Collections.Generic;

public class AIDirector : MonoBehaviour
{
    [Header("Gestión de luces")]
    public List<LightController> lights = new List<LightController>(); // Aquí arrastramos todas las luces que controla el director

    [Header("Parámetros")]
    public float decisionInterval = 5f;   // Cada cuánto tiempo toma una decisión
    public float flickerChance = 0.3f;    // Probabilidad de que una luz empiece a parpadear
    public float turnOffChance = 0.2f;    // Probabilidad de apagar una luz
    public float minIntensity = 0.5f;     // Intensidad mínima que puede fijar
    public float maxIntensity = 2f;       // Intensidad máxima que puede fijar

    private float timer = 0f;

    void Start()
    {
        // Aseguramos que todas las luces están encendidas al principio
        foreach (var light in lights)
        {
            light.SetLight(true);
            light.SetIntensity(1f);
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
                // Esta luz va a parpadear
                light.StartFlicker(Random.Range(0.05f, 0.2f));
            }
            else if (randomValue < flickerChance + turnOffChance)
            {
                // Apagamos la luz y detenemos cualquier parpadeo
                light.StopFlicker();
                light.SetLight(false);
            }
            else
            {
                // La dejamos encendida y le cambiamos la intensidad para darle vida
                light.StopFlicker();
                light.SetLight(true);
                light.SetIntensity(Random.Range(minIntensity, maxIntensity));
            }
        }
    }
}
