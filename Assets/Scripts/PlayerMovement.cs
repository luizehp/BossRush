using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;
    public int speed;

    public Animator animator;

    private Vector2 lastMoveDirection = Vector2.down;

    //Dash variables
    private bool canDash = true;
    private bool isDashing;

    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    private float x;
    private float y;
    private Vector2 moveInput;
    private bool Moving;
    public AudioSource dashSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
        if (isDashing) return;
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(x, y).normalized;
        rb.linearVelocity = moveInput * speed;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.linearVelocity = moveInput * dashingPower;
        tr.emitting = true;
        dashSound.Play();
        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;


    }



    private void Animate()
    {
        Moving = moveInput.magnitude > 0.1f;

        if (Moving)
        {
            lastMoveDirection = moveInput;
        }

        animator.SetFloat("x", lastMoveDirection.x);
        animator.SetFloat("y", lastMoveDirection.y);
        animator.SetBool("Moving", Moving);
    }


}
