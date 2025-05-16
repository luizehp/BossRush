using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public ChangeColor changeColor;
    public Camera mainCamera;
    public PlayerMovement playerMovement;
    public PlayerSlash playerSlash;
    public TridentAttack tridentAttack;
    public float phaseTransitionTime = 1f;
    public float phaseAnimationDuration = 2f;
    public int maxHealth = 100;
    public int currentHealth;
    private bool isInvincible = false;
    public float invincibilityDuration = 1f;
    public float flashInterval = 0.1f;
    private SpriteRenderer spriteRenderer;
    public bool phaseTwo = false;

    public DemonAreaAttack demonAreaAttack;
    [Header("Phase Status")]


    public Animator animator;

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

    }

    void EnterPhaseTwo()
    {
        currentPhase = BossPhase.PhaseTwo;
        phaseTwo = true;
        demonAreaAttack.enabled = true;
        StartCoroutine(PhaseTransition());
    }

    public bool IsInPhaseTwo()
    {
        return phaseTwo;
    }

    IEnumerator PhaseTransition()
    {
        // Store original camera state
        Transform originalCameraParent = mainCamera.transform.parent;
        Vector3 originalCameraLocalPosition = mainCamera.transform.localPosition;
        Vector3 originalCameraWorldPosition = mainCamera.transform.position;

        // Disable components
        GetComponent<DemonAI>().enabled = false;
        GetComponent<EnemyJumpAttack>().enabled = false;
        GetComponent<DemonFB>().enabled = false;
        tridentAttack.StopAllAttacks();

        // Stop player movement and disable collider
        playerMovement.enabled = false;
        playerSlash.enabled = false;
        Collider2D playerCollider = playerMovement.GetComponent<Collider2D>();
        bool originalColliderState = false;

        if (playerCollider != null)
        {
            originalColliderState = playerCollider.enabled;
            playerCollider.enabled = false;
        }

        Rigidbody2D playerRb = playerMovement.GetComponent<Rigidbody2D>();
        if (playerRb != null) playerRb.linearVelocity = Vector2.zero;

        // Unparent camera and move to boss
        mainCamera.transform.parent = null;
        Vector3 targetPosition = transform.position;
        targetPosition.z = originalCameraWorldPosition.z;

        // Camera transition to boss
        float elapsed = 0f;
        while (elapsed < phaseTransitionTime)
        {
            mainCamera.transform.position = Vector3.Lerp(originalCameraWorldPosition, targetPosition,
                elapsed / phaseTransitionTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Play phase transition animation
        animator.SetTrigger("PhaseTwo");
        changeColor.Change();
        yield return new WaitForSeconds(phaseAnimationDuration);

        // Calculate return position based on current player position
        Vector3 returnPosition = playerMovement.transform.position;
        returnPosition.z = originalCameraWorldPosition.z;

        // Camera transition back to player
        elapsed = 0f;
        while (elapsed < phaseTransitionTime)
        {
            mainCamera.transform.position = Vector3.Lerp(targetPosition, returnPosition,
                elapsed / phaseTransitionTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Restore camera parent and original LOCAL position
        mainCamera.transform.parent = originalCameraParent;
        mainCamera.transform.localPosition = originalCameraLocalPosition;

        // Re-enable components and restore player collider
        GetComponent<DemonAI>().enabled = true;
        GetComponent<EnemyJumpAttack>().enabled = true;
        GetComponent<DemonFB>().enabled = true;
        tridentAttack.StartAttacks();
        playerMovement.enabled = true;
        playerSlash.enabled = true;

        if (playerCollider != null)
        {
            playerCollider.enabled = originalColliderState;
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        GetComponent<DemonDeath>().Die();
        enabled = false; // Disable BossController
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
