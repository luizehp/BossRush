using System.Collections;
using UnityEngine;

namespace Necromancer.Minion
{
    public class MinionController : MonoBehaviour
    {
        [Header("Movimentação e Ataque")]
        public float speed = 2f;
        public float attackRange = 0.8f;
        public float spawnDuration = 0.7f;
        public float attackDuration = 0.5f;
        public int damageAmount = 1;            // Quanto dano o minion causa
        private Transform playerPos;
        private PlayerHealth playerHealth;
        private Animator anim;

        private enum State { Spawning, Chasing, Attacking }
        private State state;

        void Start()
        {
            // Busca o Player pela tag e pega seu PlayerHealth
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerPos = player.transform;
                playerHealth = player.GetComponent<PlayerHealth>();
            }

            anim = GetComponent<Animator>();
            state = State.Spawning;
            StartCoroutine(DoSpawn());
        }

        IEnumerator DoSpawn()
        {
            Vector3 dir = (playerPos.position - transform.position).normalized;
            bool goingUp = playerPos.position.y > transform.position.y + 0.1f;
            if (goingUp)
            {
            }
            else
            {
                anim.SetTrigger("spawnDown");
            }
            yield return new WaitForSeconds(spawnDuration);
            state = State.Chasing;
        }

        void Update()
        {
            if (state == State.Chasing && playerPos != null)
                Chase();
        }

        void Chase()
        {
            Vector3 dir = (playerPos.position - transform.position).normalized;
            bool goingUp = playerPos.position.y > transform.position.y + 0.1f;
            if (goingUp)
            {
                anim.SetBool("chaseDown", false);
                anim.SetBool("chaseUp", true);
            }
            else
            {
                anim.SetBool("chaseUp", false);
                anim.SetBool("chaseDown", true);
            }
            transform.position += dir * (speed * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, playerPos.position);
            if (distance <= attackRange && state != State.Attacking)
            {
                anim.SetBool("chaseDown", false);
                anim.SetBool("chaseUp", false);
                StartCoroutine(DoAttack());
            }
        }

        IEnumerator DoAttack()
        {
            state = State.Attacking;

            // Dispara animação de ataque
            bool goingUp = playerPos.position.y > transform.position.y + 0.1f;
            if (goingUp)
            {
                anim.SetTrigger("attackUp");
            }
            else
            {
                anim.SetTrigger("attackDown");
            }

            // 2) Aguarda fim da animação de ataque
            yield return new WaitForSeconds(attackDuration);
            
            state = State.Chasing;
        }
    }
}
