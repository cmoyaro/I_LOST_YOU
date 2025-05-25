using UnityEngine;

// Asigna una pista musical y un volumen concreto al iniciar la escena
public class SceneMusicSetter : MonoBehaviour
{
    [Header("Música de esta escena")]
    public AudioClip sceneMusicClip;

    [Range(0f, 1f)]
    public float volume = 1f;

    void Start()
    {
        if (MusicManager.Instance != null)
        {
            if (sceneMusicClip != null)
                MusicManager.Instance.ChangeTrack(sceneMusicClip);

            MusicManager.Instance.SetVolume(volume);
        }
    }
}
