using UnityEngine;

public class HeartScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.health += 1;

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.playerHealth += 1;
                }
                else
                {
                    Debug.LogWarning("GameManager.Instance está nulo!");
                }

                if (playerHealth.health > playerHealth.maxHealth)
                {
                    playerHealth.health = playerHealth.maxHealth;
                }

                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("PlayerHealth não encontrado no objeto com tag Player.");
            }
        }
    }
}
