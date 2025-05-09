using UnityEngine;

public class HeartScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is the player
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().health += 1;
            if (collision.GetComponent<PlayerHealth>().health > collision.GetComponent<PlayerHealth>().maxHealth)
            {
                collision.GetComponent<PlayerHealth>().health = collision.GetComponent<PlayerHealth>().maxHealth;
            }
            // Destroy the heart object
            Destroy(gameObject);
         
        }
    }
}
