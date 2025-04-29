using System.Collections;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    [Header("Configurações Gerais")]
    public float speed = 2f;               // Velocidade de movimento
    public float attackRange = 1.5f;       // Distância para iniciar o ataque
    public float spawnDuration = 0.7f;     // Duração (s) da animação de spawn
    public float attackDuration = 0.5f;    // Duração (s) da animação de ataque

    private Transform player;
    private Animator anim;

    private enum State { Spawning, Chasing, Attacking }
    private State state;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
            Debug.LogError("MinionController: não encontrou objeto com tag 'Player'!");

        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("MinionController: não encontrou Animator no Minion!");

        state = State.Spawning;
        StartCoroutine(DoSpawn());
    }

    IEnumerator DoSpawn()
    {
        anim.Play("MinionSpawnDown");
        yield return new WaitForSeconds(spawnDuration);
        state = State.Chasing;
    }

    void Update()
    {
        if (state == State.Chasing && player != null)
            Chase();
    }

    void Chase()
    {
        Vector3 dir = (player.position - transform.position);
        float distance = dir.magnitude;
        dir.Normalize();

        bool goingUp = player.position.y > transform.position.y + 0.1f;
        anim.Play(goingUp ? "MinionRunUp" : "MinionRunDown");

        transform.position += dir * speed * Time.deltaTime;

        if (distance <= attackRange && state != State.Attacking)
            StartCoroutine(DoAttack());
    }

    IEnumerator DoAttack()
    {
        state = State.Attacking;

        bool goingUp = player.position.y > transform.position.y + 0.1f;
        anim.Play(goingUp ? "MinionAttackUp" : "MinionAttackDown");

        yield return new WaitForSeconds(attackDuration);
        state = State.Chasing;
    }
}
