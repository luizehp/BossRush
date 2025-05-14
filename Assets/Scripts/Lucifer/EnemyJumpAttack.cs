using UnityEngine;
using System.Collections;

public class EnemyJumpAttack : MonoBehaviour
{
    [Header("Configurações")]
    public float jumpDuration = 0.5f;
    public float recoveryTime = 0.5f;
    public Animator animator;

    [Header("Área de Dano (Opcional)")]
    public GameObject damageEffectPrefab;
    public float damageRadius = 1f;

    private bool isJumping = false;
    public bool IsJumping => isJumping;

    public void StartJump(Vector2 targetPosition)
    {
        if (isJumping)
        {
            return;
        }

        Debug.Log("Iniciando pulo para: " + targetPosition);
        StartCoroutine(JumpRoutine(targetPosition));
    }

    private IEnumerator JumpRoutine(Vector2 targetPosition)
    {
        isJumping = true;
        if (animator != null)
        {
            animator.SetBool("Jumping", true);
        }

        Vector2 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < jumpDuration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsed / jumpDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        if (animator != null)
            animator.SetBool("Jumping", false);

        if (damageEffectPrefab != null)
            Instantiate(damageEffectPrefab, targetPosition, Quaternion.identity);

        yield return new WaitForSeconds(recoveryTime);
        isJumping = false;
    }
}
