using System.Collections;
using Necromancer.Tower;
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
        private GameObject towerSpawner;
        private Transform playerPos;
        private PlayerHealth playerHealth;
        private Animator anim;

        private enum State { Spawning, Chasing, Attacking }
        private State state;
        private TowerSpawner towerSpawnerController;

        [Header("Áudio de Ataque")]
        public AudioClip attackSFX;

        void Start()
        {
            // Busca o Player pela tag e pega seu PlayerHealth
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerPos = player.transform;
                playerHealth = player.GetComponent<PlayerHealth>();
            }
            towerSpawner = GameObject.FindWithTag("TowerSpawner");
            towerSpawnerController = towerSpawner.GetComponent<TowerSpawner>();
            anim = GetComponent<Animator>();
            state = State.Spawning;
            StartCoroutine(DoSpawn());
        }

        IEnumerator DoSpawn()
        {
            yield return new WaitForSeconds(spawnDuration);
            state = State.Chasing;
        }

        void Update()
        {
            if (state == State.Chasing && playerPos is not null)
                Chase();
            if (towerSpawnerController.Ended)
            {
                anim.SetTrigger("Death");
            }
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
                {
                    anim.SetBool("chaseDown", false);
                    anim.SetBool("chaseUp", false);
                    StartCoroutine(DoAttack());
                }
            }
        }

        IEnumerator DoAttack()
        {
            state = State.Attacking;

            bool goingUp = playerPos.position.y > transform.position.y + 0.1f;
            if (goingUp)
            {
                anim.SetBool("attackDown", false);
                anim.SetBool("attackUp", true);
            }
            else
            {
                anim.SetBool("attackUp", false);
                anim.SetBool("attackDown", true);
            }

            if (attackSFX != null)
                AudioSource.PlayClipAtPoint(attackSFX, transform.position, 1f);

            yield return new WaitForSeconds(attackDuration);


            state = State.Chasing;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
