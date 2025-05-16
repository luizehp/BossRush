using UnityEngine;

public class SlashCol : MonoBehaviour
{
    public bool slashCol = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) slashCol = true;
        Destroy(gameObject);
    }
}