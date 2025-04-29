using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    [Header("Referências")]
    [Tooltip("Arraste aqui o prefab do Minion")]
    public GameObject minionPrefab;

    [Header("Quantidade & Raio")]
    [Tooltip("Quantos Minions serão instanciados ao apertar V")]
    public int spawnCount = 3;
    [Tooltip("Raio em unidades ao redor do Player onde eles vão aparecer")]
    public float spawnRadius = 5f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
            Debug.LogError("MinionSpawner: não encontrou objeto com tag 'Player'!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (minionPrefab == null)
            {
                Debug.LogError("MinionSpawner: 'minionPrefab' não está atribuído no Inspector!");
                return;
            }
            if (player == null)
                return;

            for (int i = 0; i < spawnCount; i++)
            {
                // gera ponto aleatório num círculo de raio spawnRadius
                Vector2 offset = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPos = player.position + new Vector3(offset.x, offset.y, 0f);

                Instantiate(minionPrefab, spawnPos, Quaternion.identity);
            }

            Debug.Log($"MinionSpawner: instanciados {spawnCount} minions em raio {spawnRadius} ao redor de {player.name}");
        }
    }
}
