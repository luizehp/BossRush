using UnityEngine;
using System.Collections;


public class PlayerSlash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool isSlashing;
    public Animator animator;
    private bool canSlash = true;
    float lastMoveX;
    float lastMoveY;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput != Vector2.zero)
        {
            lastMoveX = Input.GetAxisRaw("Horizontal");
            lastMoveY = Input.GetAxisRaw("Vertical");
        }

        if (Input.GetKeyDown(KeyCode.Z) && canSlash)
        {
            StartCoroutine(Slash(lastMoveX, lastMoveY));
        }

    }

    private IEnumerator Slash(float x, float y)
    {
        canSlash = false;
        isSlashing = true;

        animator.SetFloat("AttackX", x);
        animator.SetFloat("AttackY", y);
        animator.SetBool("Slashing", isSlashing);
        yield return new WaitForSeconds(0.4f);
        isSlashing = false;
        animator.SetBool("Slashing", isSlashing);
        yield return new WaitForSeconds(0.1f);
        canSlash = true;
    }
}
