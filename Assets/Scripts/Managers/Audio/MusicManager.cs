using UnityEngine;

// Controlador global de música basado en el patrón Singleton
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip musicClip;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void Play()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void Pause()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }

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
