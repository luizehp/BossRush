using UnityEngine;
using System.Collections;
using UnityEditor.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health = 5;
    public int maxHealth;
    public float invincibilityDuration = 1f;
    public bool isInvincible = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool died = false;
    private Animator playerAnimator;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    void Start()
    {
        maxHealth = health;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Collider2D selfCollider = GetComponent<Collider2D>();
        bool selfIsTrigger = selfCollider.isTrigger;
        bool otherIsTrigger = other.isTrigger;

        if (!isInvincible && other.CompareTag("EnemyAttack"))
        {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Max(health, 0);
        Debug.Log($"Player hit! Health: {health}/{maxHealth}");
        if (died)
        {
            return;
        }

        if (health <= 0)
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
        animator = GetComponent<Animator>();
        animator.SetTrigger("Death");
        playerMovement.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        playerAnimator.SetBool("Moving", false);
        died = true;
    }
}
