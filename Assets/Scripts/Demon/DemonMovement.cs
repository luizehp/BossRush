using UnityEngine;

public class DemonMovement : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public float speed = 3f;
    public float chaseRadius = 10f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 currentPos = transform.position;
        Vector2 targetPos = player.position;

        float dist = Vector2.Distance(currentPos, targetPos);

        if (dist <= chaseRadius)
        {
            Vector2 direction = (targetPos - currentPos).normalized;

            transform.position = Vector2.MoveTowards(
                currentPos,
                targetPos,
                speed * Time.deltaTime
            );

            animator.SetFloat("x", direction.x);
            animator.SetFloat("y", direction.y);
        }
    }
}
