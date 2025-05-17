using UnityEngine;
using System.Collections;

public class EnemyJumpAttack : MonoBehaviour
{
    public float jumpDuration = 0.5f;
    public float recoveryTime = 0.5f;
    public Animator animator;
    public BossController bossController;

    private bool isJumping = false;
    public bool IsJumping => isJumping;
    public AudioSource audioSource;
    public AudioClip jumpSound;

    public void StartJump(Vector2 targetPosition)
    {
        if (isJumping || bossController.IsInPhaseTwo())
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
        bool sound = false;

        while (elapsed < jumpDuration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsed / jumpDuration);
            elapsed += Time.deltaTime;
            if (elapsed >= 1f && !sound)
            {
                audioSource.clip = jumpSound;
                audioSource.Play();
                sound = true;
            }
            yield return null;
        }


        transform.position = targetPosition;

        if (animator != null)
            animator.SetBool("Jumping", false);

        yield return new WaitForSeconds(recoveryTime);
        isJumping = false;
    }
}
