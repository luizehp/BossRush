using UnityEngine;
using System.Collections;


public class PlayerSlash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool isSlashing;
    public Animator animator;
    private bool canSlash = true;
    public Slash slashScript;
    private Vector2 lastDirection = Vector2.right;
    public AudioSource SlashSource;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (moveInput != Vector2.zero)
        {
            lastDirection = moveInput;
        }
        if (Input.GetKeyDown(KeyCode.Z) && canSlash)
        {
            slashScript.SlashPre(lastDirection.normalized);
            StartCoroutine(Slash());
        }

    }

    private IEnumerator Slash()
    {
        canSlash = false;
        isSlashing = true;

        animator.SetFloat("AttackX", lastDirection.x);
        animator.SetFloat("AttackY", lastDirection.y);
        animator.SetBool("Slashing", isSlashing);
        SlashSource.Play();
        yield return new WaitForSeconds(0.4f);
        isSlashing = false;
        animator.SetBool("Slashing", isSlashing);
        yield return new WaitForSeconds(0.1f);
        canSlash = true;
    }
}
