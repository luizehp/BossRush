using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 5;

    [Header("Invincibility Frames")]
    [Tooltip("Seconds of immunity after getting hit")]
    public float invincibilityDuration = 1f;
    private bool isInvincible = false;

    // Optional: reference to sprite renderer for flash effect
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if collided with an "EnemyAttack" trigger
        if (!isInvincible && other.CompareTag("EnemyAttack"))
        {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage)
    {
        maxHealth -= damage;
        maxHealth = Mathf.Max(maxHealth, 0);
        Debug.Log($"Player hit! Health: {maxHealth}/{maxHealth}");

        // TODO: Play damage animation or sound here
        // animator.SetTrigger("Hurt");

        if (maxHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        // Optional: flash the sprite
        float flashInterval = 0.1f;
        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }
        spriteRenderer.enabled = true;

        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // TODO: Trigger death animation, disable input, reload scene, etc.
        // animator.SetBool("Dead", true);
        // GetComponent<PlayerController>().enabled = false;
    }

    // Optional: public method to heal the player
}
