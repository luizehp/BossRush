using System.Collections;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

namespace Necromancer.Tower
{
    public class TowerSpawner : MonoBehaviour
    {
        public GameObject prefab;
        public int quantidade = 5;
        public float distanciaMinima = 1f;
        public GameObject necromancer;
        public GameObject player;
        public Transform mainCamera;
        public float duracaoTransicao = 2f;
        public bool Ended;
        public GameObject collectable;

        public List<GameObject> instancias = new();
        private bool todasDestruidas = false;
        private Animator playerAnimator;
        private Rigidbody2D rb;
        private PlayerMovement playerMovement;
        public GameObject portalPrefab;
        public float portalOffsetY = 1.5f;

        void Start()
        {
            playerAnimator = player.GetComponent<Animator>();
            rb = player.GetComponent<Rigidbody2D>();
            playerMovement = player.GetComponent<PlayerMovement>();
            Ended = false;
            BoxCollider2D box = GetComponent<BoxCollider2D>();

            Vector2 centro = transform.position;
            float xMin = centro.x - box.size.x / 2f;
            float xMax = centro.x + box.size.x / 2f;
            float yMin = centro.y - box.size.y / 2f;
            float yMax = centro.y + box.size.y / 2f;

            int tentativasMax = 100;

            for (int i = 0; i < quantidade; i++)
            {
                bool encontrou = false;
                Vector2 novaPos = Vector2.zero;
                int tentativas = 0;

                while (!encontrou && tentativas < tentativasMax)
                {
                    float x = Random.Range(xMin, xMax);
                    float y = Random.Range(yMin, yMax);
                    novaPos = new Vector2(x, y);

                    encontrou = true;
                    foreach (GameObject go in instancias)
                    {
                        if (Vector2.Distance(novaPos, go.transform.position) < distanciaMinima)
                        {
                            encontrou = false;
                            break;
                        }
                    }

                    tentativas++;
                }

                if (encontrou)
                {
                    GameObject torre = Instantiate(prefab, novaPos, Quaternion.identity);
                    instancias.Add(torre);
                }
            }
        }

        void Update()
        {
            if (!todasDestruidas && instancias.TrueForAll(obj => obj == null)) 
            {
                todasDestruidas = true;
                StartCoroutine(AllDead());
            }
        }

        IEnumerator AllDead()
        {
            mainCamera.SetParent(null);
            player.GetComponent<PlayerHealth>().isInvincible = true;
            necromancer.GetComponent<BehaviorGraphAgent>().End();
            Ended = true;
            yield return StartCoroutine(MoveCameraTowards(necromancer.transform.position));
            necromancer.GetComponent<Animator>().SetTrigger("Death");
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(MoveCameraTowards(player.transform.position));
            player.GetComponent<PlayerHealth>().isInvincible = false;
            mainCamera.SetParent(player.transform);
            
            Vector3 centerPos = transform.position;
            float safeDistance = 1.5f;
            float offsetY = 2f;

            bool playerEstaPerto = Vector3.Distance(player.transform.position, centerPos) < safeDistance;

            Vector3 spawnPos;
            if (playerEstaPerto)
            {
                spawnPos = centerPos + new Vector3(0, offsetY, 0);
            }
            else
            {
                spawnPos = centerPos;
            }

            Instantiate(collectable, spawnPos, Quaternion.identity);
            
            Vector3 portalPos = necromancer.transform.position - new Vector3(0, portalOffsetY, 0);
            Instantiate(portalPrefab, portalPos, Quaternion.identity);
        }
    
        IEnumerator MoveCameraTowards(Vector3 destino)
        {
            playerMovement.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
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
            
            playerMovement.enabled = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.freezeRotation = true;
        }
    }
}
