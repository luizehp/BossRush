using UnityEngine;

public class Flame : MonoBehaviour
{
    public ParticleSystem flame1;
    public ParticleSystem flame2;

    public GameObject light1;
    public GameObject light2;
    public AudioSource audioSource;
    public AudioClip lightSound;
    private bool isFlameActive = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFlameActive)
        {
            isFlameActive = true;
            audioSource.clip = lightSound;
            audioSource.volume = 0.4f;
            audioSource.Play();
            light1.SetActive(true);
            light2.SetActive(true);
            var emission1 = flame1.emission;
            emission1.enabled = true;
            var emission2 = flame2.emission;
            emission2.enabled = true;
        }
    }
}