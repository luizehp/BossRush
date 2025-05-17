using UnityEngine;
using System.Collections;

public class SlashCol : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip item;
    private void OnTriggerEnter2D(Collider2D other)
    {
        audioSource.clip = item;
        audioSource.Play();
        if (other.CompareTag("Player"))
        {
            Debug.Log("Slash collected");
            if (GameManager.Instance != null) GameManager.Instance.hasSlashAbility = true;
            Destroy(gameObject);
        }
    }
}