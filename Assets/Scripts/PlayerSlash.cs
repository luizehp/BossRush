using UnityEngine;
using System.Collections;


public class PlayerSlash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool isSlashing;
    public Animator animator;
    private bool canSlash = true;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z) && canSlash)
        {
            StartCoroutine(Slash());
        }

    }

    private IEnumerator Slash()
    {
        canSlash = false;
        isSlashing = true;

        animator.SetFloat("AttackX", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("AttackY", Input.GetAxisRaw("Vertical"));
        animator.SetBool("Slashing", isSlashing);
        yield return new WaitForSeconds(0.4f);
        isSlashing = false;
        animator.SetBool("Slashing", isSlashing);
        yield return new WaitForSeconds(0.1f);
        canSlash = true;
    }
}
