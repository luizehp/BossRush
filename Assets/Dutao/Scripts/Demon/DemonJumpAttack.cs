using UnityEngine;
using System.Collections;


public class DemonJumpAttack : MonoBehaviour
{

    private bool canJump = true;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.B) && canJump)
        {
            StartCoroutine(Jump());
        }

    }

    private IEnumerator Jump()
    {
        canJump = false;
        animator.SetBool("Jumping", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Jumping", false);
        yield return new WaitForSeconds(0.4f);
        canJump = true;
    }
}
