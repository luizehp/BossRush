using UnityEngine;
using System.Collections;

public class DemonAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    private Animator animator;
    private EnemyJumpAttack jumpAttack;

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
        jumpAttack = GetComponent<EnemyJumpAttack>();
    }

    void Update()
    {
        if (jumpAttack != null && jumpAttack.IsJumping)
            return; // Não faz nada se estiver pulando

        float dist = Vector2.Distance(transform.position, player.position);

        // Trigger de teste: jogador aperta B e inimigo faz o pulo
        if (Input.GetKeyDown(KeyCode.B))
        {
            Vector2 target = player.position;
            jumpAttack.StartJump(target);
            return;
        }

        switch (currentState)
        {
            case State.Idle:
                animator.SetBool("Walking", false);
                if (dist <= chaseRadius)
                    currentState = State.Chase;
                break;

            case State.Chase:
                animator.SetBool("Attacking", false);
                if (dist > chaseRadius)
                {
                    currentState = State.Idle;
                }
                else if (dist <= attackRadius && !isAttacking)
                {
                    currentState = State.Attack;
                    StartCoroutine(PerformAttack());
                }
                else
                {
                    ChasePlayer();
                }
                break;

            case State.Attack:
                // ataque controlado pela coroutine
                break;
        }
    }

    private void ChasePlayer()
    {
        Vector2 currentPos = transform.position;
        Vector2 dir = ((Vector2)player.position - currentPos).normalized;

        transform.position = Vector2.MoveTowards(currentPos, player.position, speed * Time.deltaTime);
        animator.SetFloat("x", dir.x);
        animator.SetFloat("y", dir.y);
        animator.SetBool("Walking", true);
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        animator.SetFloat("x", dir.x);
        animator.SetFloat("y", dir.y);

        animator.SetBool("Walking", false);
        animator.SetBool("Attacking", true);

        yield return new WaitForSeconds(attackDuration);

        animator.SetBool("Attacking", false);

        yield return new WaitForSeconds(recoveryDuration);

        isAttacking = false;
        currentState = State.Chase;
    }
}
