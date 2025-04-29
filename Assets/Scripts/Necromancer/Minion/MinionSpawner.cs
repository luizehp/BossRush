using UnityEngine;

namespace Necromancer.Minion
{
    public class MinionSpawner : MonoBehaviour
    {
        public GameObject minionPrefab;
        public int spawnCount = 3;
        public float spawnRadius = 5f;
        private Transform playerPos;
        private Animator necromancerAnimator;

        void Start()
        {
            necromancerAnimator = GetComponent<Animator>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player is not null)
                playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (playerPos is null)
                    return;
                necromancerAnimator.SetTrigger("Summon");
            }
        }

        public void SpawnMinions()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Vector2 offset = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPos = playerPos.position + new Vector3(offset.x, offset.y, 0f);

                Instantiate(minionPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}
