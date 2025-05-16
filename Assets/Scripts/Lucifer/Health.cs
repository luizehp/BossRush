using System;
using System.Collections;
using UnityEngine;

public partial class Health : MonoBehaviour
{
    public int attackDamage = 10;
    public int maxHealth = 100;
    public float invincibilityDuration = 1f;
    public float flashInterval = 0.1f;
    public AudioSource deathSFX;
    private int currentHealth;
    private bool isInvincible = false;
    private bool isDead = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInvincible && other.CompareTag("Sword") && !isDead)
        {
            currentHealth -= attackDamage;
            StartCoroutine(InvincibilityCoroutine());
        }
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("Death");
            if (deathSFX != null)
                deathSFX.Play();
        }
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

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
