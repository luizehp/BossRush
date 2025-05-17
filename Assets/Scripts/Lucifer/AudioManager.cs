using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip Music;
    public void StartMusic()
    {
        audioSource.clip = Music;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void EndMusic()
    {
        audioSource.Stop();
    }
}
