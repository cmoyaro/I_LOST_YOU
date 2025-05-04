using UnityEngine;

// Este script controla la música del juego usando el patrón Singleton
// Se asegura de que solo haya una instancia que persista entre escenas
public class MusicManager : MonoBehaviour
{
    // Instancia global accesible desde cualquier parte del código
    public static MusicManager Instance { get; private set; }

    [Header("Audio Settings")]
    public AudioSource audioSource;   // Componente que reproduce la música
    public AudioClip musicClip;       // Pista de música que queremos reproducir

    void Awake()
    {
        // Si ya hay una instancia (y no somos nosotros), destruimos este objeto para mantener solo uno
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Si somos la primera instancia, nos guardamos como tal
        Instance = this;

        // Hacemos que este objeto no se destruya al cambiar de escena
        DontDestroyOnLoad(gameObject);

        // Si no se ha asignado un AudioSource desde el Inspector, lo creamos automáticamente
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Si hay una pista asignada, la configuramos para que suene en bucle y la reproducimos
        if (musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Reproduce la música si no está sonando
    public void Play()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    // Pausa la música si está sonando
    public void Pause()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }

    // Detiene completamente la música
    public void Stop()
    {
        audioSource.Stop();
    }

    // Cambia el volumen de la música. El valor debe estar entre 0 (silencio) y 1 (máximo)
    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume); // Clamp por seguridad
    }

    // Cambia la pista de música. Se detiene la anterior y se reproduce la nueva.
    public void ChangeTrack(AudioClip newClip)
    {
        if (newClip != null)
        {
            audioSource.Stop();
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}
