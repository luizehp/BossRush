using System.Collections;
using UnityEngine;

namespace Necromancer.Minion
{
    public class MinionController : MonoBehaviour
    {
        public float speed = 2f;
        public float attackRange = 1.5f;
        public float spawnDuration = 0.7f;
        public float attackDuration = 0.5f;

        private Transform playerPos;
        private Animator anim;

        private enum State { Spawning, Chasing, Attacking }
        private State state;

        void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player is not null)
                playerPos = GameObject.FindGameObjectWithTag("Player").transform;

            anim = GetComponent<Animator>();

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
            if (state == State.Chasing && playerPos is not null)
                Chase();
        }

        void Chase()
        {
            Vector3 dir = (playerPos.position - transform.position);
            float distance = dir.magnitude;
            dir.Normalize();

            bool goingUp = playerPos.position.y > transform.position.y + 0.1f;
            anim.Play(goingUp ? "MinionRunUp" : "MinionRunDown");

            transform.position += dir * (speed * Time.deltaTime);

            if (distance <= attackRange && state != State.Attacking)
                StartCoroutine(DoAttack());
        }

        IEnumerator DoAttack()
        {
            state = State.Attacking;

            bool goingUp = playerPos.position.y > transform.position.y + 0.1f;
            anim.Play(goingUp ? "MinionAttackUp" : "MinionAttackDown");

            yield return new WaitForSeconds(attackDuration);
            state = State.Chasing;
        }
    }
}
