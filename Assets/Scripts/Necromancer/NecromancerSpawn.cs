using System.Collections;
using Unity.Behavior;
using UnityEngine;

namespace Necromancer
{
    public class NecromancerSpawn : MonoBehaviour
    {
        public Transform mainCamera;
        public Transform player;
        public GameObject necromancer;
        public float duracaoTransicao = 1.5f;
        private bool spawningEnded = false;
        public bool isSpawnFinished = false;

        private PlayerMovement playerMovement;
        private Rigidbody2D rb;
        private Animator playerAnimator;
        private BehaviorGraphAgent btAgent;
        public AudioSource backgroundAudio;
        public AudioSource spawningAudio;
        public AudioSource surgingAudio;
        private PlayerSlash playerAttack;

        private Animator animator;

        void Start()
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerAttack = player.GetComponent<PlayerSlash>();
            rb = player.GetComponent<Rigidbody2D>();
            playerAnimator = player.GetComponent<Animator>();
            isSpawnFinished = false;
            btAgent = GetComponent<BehaviorGraphAgent>();
            StartSpawningRoutine();
        }
        
        public void StartSpawningRoutine()
        {
            if (isSpawnFinished) return;
            StartCoroutine(SpawningRoutine());
        }
        
        public void OnSpawningAnimationEnd()
        {
            spawningEnded = true;
            backgroundAudio.enabled = true;
        }
        
        IEnumerator SpawningRoutine()
        {
            spawningAudio.Play();
            mainCamera.SetParent(null);
            player.GetComponent<PlayerHealth>().isInvincible = true;
            necromancer.GetComponent<BehaviorGraphAgent>().End();
            yield return StartCoroutine(MoveCameraTowards(necromancer.transform.position));
            necromancer.GetComponent<Animator>().SetTrigger("Spawning");
            surgingAudio.Play();
            yield return new WaitUntil(() => spawningEnded);
            yield return StartCoroutine(MoveCameraTowards(player.transform.position));
            playerMovement.enabled = true;
            playerAttack.enabled = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.freezeRotation = true;
            player.GetComponent<PlayerHealth>().isInvincible = false;
            mainCamera.SetParent(player.transform);
            isSpawnFinished = true;
            btAgent.enabled = true;
        }
    
        IEnumerator MoveCameraTowards(Vector3 destino)
        {
            playerMovement.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            playerAttack.enabled = false;
            playerAnimator.SetBool("Moving", false);
            
            Vector3 origem = mainCamera.position;
            destino.z = origem.z;

            float tempo = 0f;
            while (tempo < duracaoTransicao)
            {
                mainCamera.position = Vector3.Lerp(origem, destino, tempo / duracaoTransicao);
                tempo += Time.deltaTime;
                yield return null;
            }

            mainCamera.position = destino;
        }

    }
}
