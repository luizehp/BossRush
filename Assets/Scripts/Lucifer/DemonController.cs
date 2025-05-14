using System;
using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private bool isInvincible = false;
    public float invincibilityDuration = 1f;
    public float flashInterval = 0.1f;
    private SpriteRenderer spriteRenderer;
    public bool phaseTwo = false; // Flag to indicate if in phase two


    public Animator animator; // Assign via Inspector

    private enum BossPhase { PhaseOne, PhaseTwo }
    private BossPhase currentPhase = BossPhase.PhaseOne;

    void Start()
    {
        currentHealth = maxHealth;
        currentPhase = BossPhase.PhaseOne;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInvincible && other.CompareTag("Sword"))
        {
            currentHealth -= 10;
            StartCoroutine(InvincibilityCoroutine());
        }
        if (currentHealth <= maxHealth / 2 && currentPhase == BossPhase.PhaseOne)
        {
            Debug.Log("Entering Phase Two!");
            EnterPhaseTwo();
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void EnterPhaseTwo()
    {
        currentPhase = BossPhase.PhaseTwo;
        phaseTwo = true;

        // Trigger animation change
        animator.SetTrigger("PhaseTwo");

        // Change attack pattern, movement, speed etc.
        // Example: GetComponent<YourAttackScript>().SwitchToPhaseTwo();
    }

    void Die()
    {
        animator.SetTrigger("Die");
        Destroy(gameObject, 2f);
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

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
}
