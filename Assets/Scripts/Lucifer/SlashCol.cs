using UnityEngine;

public class SlashCol : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.hasSlashAbility = true;
        Destroy(gameObject);
    }
}