using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    [Header("Gestión de luces")]
    public List<LightController> lights = new List<LightController>();

    [Header("Parámetros de decisión")]
    public float decisionInterval = 5f;
    public float flickerChance = 0.3f;
    public float turnOffChance = 0.2f;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;

    [Header("Parámetros del titileo")]
    public float flickerSpeedMin = 0.05f;
    public float flickerSpeedMax = 0.08f;
    public float flickerAmountMin = 0.05f;
    public float flickerAmountMax = 0.12f;

    private float timer = 0f;

    [Header("Evento paranormal: control de activación")]
    private float lastParanormalEventTime = -Mathf.Infinity;
    public float paranormalCooldown = 20f;
    private bool isParanormalEventActive = false;

    [Header("Jugador")]
    public GameObject player;

    [Header("Audio paranormal")]
    public AudioSource ambientAudioSource;
    public AudioClip breathInClip;

    void Start()
    {
        foreach (var light in lights)
        {
            if (light == null) continue;

            light.SetLight(true);
            float initialIntensity = Random.Range(minIntensity, maxIntensity);
            light.SetIntensity(initialIntensity);
            light.StopFlicker();
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

    private void MakeDecision()
    {
        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        if (fpc == null || !fpc.HasFlashlight()) return;

        foreach (var light in lights)
        {
            float randomValue = Random.value;

            if (randomValue < flickerChance)
            {
                float speed = Random.Range(flickerSpeedMin, flickerSpeedMax);
                float amount = Random.Range(flickerAmountMin, flickerAmountMax);
                light.SetLight(true);
                light.StartFlicker(speed, amount);
            }
            else if (randomValue < flickerChance + turnOffChance)
            {
                light.StopFlicker();
                light.SetLight(false);
            }
            else
            {
                light.StopFlicker();
                light.SetLight(true);
                float newIntensity = Random.Range(minIntensity, maxIntensity);
                light.SetIntensity(newIntensity);
            }
        }

        if (Time.time - lastParanormalEventTime >= paranormalCooldown)
        {
            if (!isParanormalEventActive && Random.value < 0.08f)
            {
                ForceFlashlightGlitchAndAffectArea(8f, 1.8f, 0.07f);
                lastParanormalEventTime = Time.time;
            }
        }
    }

    public void ForceFlashlightGlitchAndAffectArea(float radius = 10f, float duration = 1.5f, float blinkInterval = 0.1f)
    {
        if (isParanormalEventActive || player == null) return;

        isParanormalEventActive = true;

        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        if (fpc == null) return;

        if (fpc.IsFlashlightOn())
        {
            fpc.ForceFlashlightShutdown(duration, blinkInterval);
        }

        if (ambientAudioSource != null && breathInClip != null)
        {
            ambientAudioSource.PlayOneShot(breathInClip);
        }

        foreach (LightController light in lights)
        {
            if (light == null) continue;

            float dist = Vector3.Distance(player.transform.position, light.transform.position);
            if (dist <= radius)
            {
                float rand = Random.value;

                if (rand < 0.5f)
                {
                    light.StartFlicker(blinkInterval, 0.15f);
                }
                else
                {
                    StartCoroutine(TemporaryLightOff(light, duration));
                }
            }
        }

        StartCoroutine(EndParanormalEventAfter(duration + 0.5f));
    }

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
    }
}
