using UnityEngine;

public class deathBeholder : MonoBehaviour
{
    public GameObject portaPrefab;     // Prefab da porta
    private Animator animator;         // Referência ao Animator do próprio GameObject
    private bool portaInvocada = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Verifica se o estado atual tem a tag "Death"
        if (!portaInvocada && animator.GetCurrentAnimatorStateInfo(0).IsTag("Death"))
        {
            InvocarPorta();
        }
    }

    void InvocarPorta()
    {
        Instantiate(portaPrefab, transform.position, Quaternion.identity);
        portaInvocada = true;
    }
}
