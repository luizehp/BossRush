using UnityEngine;
using System.Collections;

public class DemonAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    private Animator animator;

    [Header("Movement Settings")]
    public float speed = 3f;
    public float chaseRadius = 10f;

    [Header("Attack Settings")]
    public float attackRadius = 1f;
    public float attackDuration = 1f;
    public float recoveryDuration = 1f;

    private enum State { Idle, Chase, Attack }
    private State currentState = State.Idle;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                animator.SetBool("Walking", false);
                if (dist <= chaseRadius)
                {
                    currentState = State.Chase;
                    // Debug.Log("Transition to Chase");
                }
                break;

            case State.Chase:
                // Cancel any attack flag if somehow still set
                animator.SetBool("Attacking", false);

                if (dist > chaseRadius)
                {
                    currentState = State.Idle;
                    // Debug.Log("Transition to Idle");
                }
                else if (dist <= attackRadius && !isAttacking)
                {
                    currentState = State.Attack;
                    StartCoroutine(PerformAttack());
                    // Debug.Log("Transition to Attack");
                }
                else
                {
                    ChasePlayer();
                }
                break;

            case State.Attack:
                // Do nothing: the coroutine handles attack & recovery
                break;
        }
    }

    private void ChasePlayer()
    {
        Vector2 currentPos = transform.position;
        Vector2 dir = ((Vector2)player.position - currentPos).normalized;

        // Move
        transform.position = Vector2.MoveTowards(currentPos, (Vector2)player.position, speed * Time.deltaTime);

        // Animate
        animator.SetFloat("x", dir.x);
        animator.SetFloat("y", dir.y);
        animator.SetBool("Walking", true);
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        // Face the player when starting the attack
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        animator.SetFloat("x", dir.x);
        animator.SetFloat("y", dir.y);

        // Play attack
        animator.SetBool("Walking", false);
        animator.SetBool("Attacking", true);

        yield return new WaitForSeconds(attackDuration);

        // End attack animation
        animator.SetBool("Attacking", false);

        // Recovery delay
        yield return new WaitForSeconds(recoveryDuration);

        // Resume chasing
        isAttacking = false;
        currentState = State.Chase;
        // Debug.Log("Attack complete, back to Chase");
    }
}
