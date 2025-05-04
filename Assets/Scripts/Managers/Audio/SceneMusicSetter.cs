using UnityEngine;

public class SceneMusicSetter : MonoBehaviour
{
    [Header("Asignar música para esta escena")]
    public AudioClip sceneMusicClip;  // Ponemos la canción que queramos

    void Start()
    {
        if (MusicManager.Instance != null && sceneMusicClip != null)
        {
            MusicManager.Instance.ChangeTrack(sceneMusicClip);
        }
    }
}
