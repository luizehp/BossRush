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

        void Start()
        {
            // Busca o Player pela tag e pega seu PlayerHealth
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerPos = player.transform;
                playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth == null)
                    Debug.LogWarning("PlayerHealth não encontrado no Player!");
            }
            towerSpawner = GameObject.FindWithTag("TowerSpawner");
            towerSpawnerController = towerSpawner.GetComponent<TowerSpawner>();
            anim = GetComponent<Animator>();
            state = State.Spawning;
            StartCoroutine(DoSpawn());

            // Garante que o hitbox começa desativado
            if (attackHitbox != null)
                attackHitbox.SetActive(false);
        }

        IEnumerator DoSpawn()
        {
            anim.Play("MinionSpawnDown");
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
            anim.Play(goingUp ? "MinionRunUp" : "MinionRunDown");
            transform.position += dir * (speed * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, playerPos.position);
            if (distance <= attackRange && state != State.Attacking)
                StartCoroutine(DoAttack());
        }

        IEnumerator DoAttack()
        {
            state = State.Attacking;

            // Dispara animação de ataque
            bool goingUp = playerPos.position.y > transform.position.y + 0.1f;
            anim.Play(goingUp ? "MinionAttackUp" : "MinionAttackDown");

            // 1) Ativa o hitbox para causar trigger no PlayerHealth
            if (attackHitbox != null)
                attackHitbox.SetActive(true);

            // 2) Aguarda fim da animação de ataque
            yield return new WaitForSeconds(attackDuration);

            // 3) Desativa o hitbox
            if (attackHitbox != null)
                attackHitbox.SetActive(false);

            state = State.Chasing;
        }

    }
}
