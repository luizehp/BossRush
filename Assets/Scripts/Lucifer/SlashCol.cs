using UnityEngine;

public class SlashCol : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null) GameManager.Instance.hasSlashAbility = true;
            Destroy(gameObject);
        }
    }
}