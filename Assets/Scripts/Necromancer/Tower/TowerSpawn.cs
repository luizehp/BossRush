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

        [Header("Beam Settings")]
        public GameObject beamPrefab;
        public int beamSegments = 20;
        public float beamAmplitude = 0.5f;
        public float beamFrequency = 1f;
        public float beamSpeed = 1f;
        public float scrollSpeed = 2f;
        private List<LineRenderer> beams      = new List<LineRenderer>();
        private List<LineRenderer> noiseBeams = new List<LineRenderer>();
        private List<Light>      beamLights   = new List<Light>();

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
            rb             = player.GetComponent<Rigidbody2D>();
            playerMovement = player.GetComponent<PlayerMovement>();
            Ended          = false;

            BoxCollider2D box = GetComponent<BoxCollider2D>();
            Vector2 centro    = transform.position;
            float xMin        = centro.x - box.size.x / 2f;
            float xMax        = centro.x + box.size.x / 2f;
            float yMin        = centro.y - box.size.y / 2f;
            float yMax        = centro.y + box.size.y / 2f;
            int tentativasMax = 100;

            for (int i = 0; i < quantidade; i++)
            {
                bool encontrou = false;
                Vector2 novaPos = Vector2.zero;
                int tentativas  = 0;

                while (!encontrou && tentativas < tentativasMax)
                {
                    float x = Random.Range(xMin, xMax);
                    float y = Random.Range(yMin, yMax);
                    novaPos = new Vector2(x, y);
                    encontrou = true;

                    foreach (GameObject go in instancias)
                        if (Vector2.Distance(novaPos, go.transform.position) < distanciaMinima)
                        {
                            encontrou = false;
                            break;
                        }

                    tentativas++;
                }

                if (encontrou)
                {
                    GameObject torre   = Instantiate(prefab, novaPos, Quaternion.identity);
                    instancias.Add(torre);

                    GameObject beamObj = Instantiate(beamPrefab);
                    var lrs            = beamObj.GetComponentsInChildren<LineRenderer>();

                    // ←←← Aqui apenas o necessário para garantir que o beam fique em Order in Layer -1
                    foreach (var lr in lrs)
                    {
                        lr.sortingLayerName = "Default";
                        lr.sortingOrder     = -1;
                    }

                    LineRenderer mainBeam  = lrs[0];
                    LineRenderer noiseBeam = lrs[1];

                    mainBeam.positionCount = beamSegments;
                    mainBeam.textureMode   = LineTextureMode.Tile;
                    beams.Add(mainBeam);

                    noiseBeam.positionCount = beamSegments;
                    noiseBeam.textureMode   = LineTextureMode.Tile;
                    noiseBeams.Add(noiseBeam);

                    Light lt = beamObj.AddComponent<Light>();
                    lt.type      = LightType.Point;
                    lt.color     = new Color(0x15/255f, 0xFD/255f, 0xD0/255f);
                    lt.range     = 3f;
                    lt.intensity = 2f;
                    beamLights.Add(lt);
                }
            }
        }

        void Update()
        {
            // remove beams de torres destruídas
            for (int i = instancias.Count - 1; i >= 0; i--)
            {
                if (instancias[i] == null)
                {
                    Destroy(beams[i].gameObject);
                    beams.RemoveAt(i);
                    noiseBeams.RemoveAt(i);
                    beamLights.RemoveAt(i);
                    instancias.RemoveAt(i);
                }
            }

            // atualiza beams ativos
            for (int i = 0; i < instancias.Count; i++)
            {
                Vector3 torrePos = instancias[i].transform.position;
                Vector3 bossPos  = necromancer.transform.position;
                Vector3 dir      = bossPos - torrePos;
                Vector3 perp     = Vector3.Cross(dir.normalized, Vector3.forward);

                for (int s = 0; s < beamSegments; s++)
                {
                    float t       = (float)s / (beamSegments - 1);
                    Vector3 basePos = Vector3.Lerp(torrePos, bossPos, t);
                    float noiseVal = (Mathf.PerlinNoise(t * beamFrequency + Time.time * beamSpeed, 0f) - 0.5f) * 2f;
                    basePos += perp * noiseVal * beamAmplitude * Mathf.Sin(Mathf.PI * t);

                    beams[i].SetPosition(s, basePos);
                    noiseBeams[i].SetPosition(s, basePos);
                }

                float offset = Time.time * scrollSpeed;
                beams[i].material.mainTextureOffset      = new Vector2(-offset, 0f);
                noiseBeams[i].material.mainTextureOffset = new Vector2(-offset * 1.5f, 0f);

                Vector3 mid = Vector3.Lerp(torrePos, bossPos, 0.5f);
                beamLights[i].transform.position = mid;
                beamLights[i].intensity          = 1f + Mathf.PerlinNoise(Time.time * 3f, i * 10f) * 0.5f;
            }

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
            Vector3 spawnPos = playerEstaPerto
                ? centerPos + new Vector3(0, offsetY, 0)
                : centerPos;

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
