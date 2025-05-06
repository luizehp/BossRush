using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Necromancer.Tower;  // para acessar TowerSpawner :contentReference[oaicite:0]{index=0}:contentReference[oaicite:1]{index=1}

public class HUDController : MonoBehaviour
{
    [Header("Referências - Player")]
    public GameObject player;
    public GameObject heartPrefab;
    public Transform heartsParent;

    [Header("Boss Health Bar")]
    public GameObject boss;            // SpawnableSpace ou outro objeto que controla as torres
    public Image bossFillImage;        // o Image "Fill" do seu fundo de barra
    public int bossMaxHealth = 100;    // ajuste no Inspector para definir vida máxima do boss

    private PlayerHealth playerHealth;
    private List<Image> hearts = new List<Image>();

    private FieldInfo currentHealthField;
    private TowerSpawner towerSpawner;
    private Health bossHealth;

    void Awake()
    {
        // -- PlayerHealth (corações) --
        playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            Debug.LogError("HUDController: não encontrou PlayerHealth no player!");

        // -- Reflection para ler private currentHealth em Health.cs --
        currentHealthField = typeof(Health)
            .GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance);

        // -- TowerSpawner (boss de torres) ou Health (boss comum) --
        towerSpawner = boss.GetComponent<TowerSpawner>();
        bossHealth   = boss.GetComponent<Health>();  // caso você queira suportar bosses “normais”
    }

    void Start()
    {
        // Instancia os corações iniciais de acordo com playerHealth.health
        for (int i = 0; i < playerHealth.health; i++)
        {
            var go = Instantiate(heartPrefab, heartsParent);
            hearts.Add(go.GetComponent<Image>());
        }

        // Deixa a barra do boss cheia (1 = 100%)
        if (bossFillImage != null)
            bossFillImage.fillAmount = 1f;
    }

    void Update()
    {
        AtualizaQuantidadeDeCoracoes();
        AtualizaBossBar();
    }

    private void AtualizaQuantidadeDeCoracoes()
    {
        int current = playerHealth.health;
        // adiciona ou remove corações conforme a vida
        if (hearts.Count < current)
        {
            for (int i = hearts.Count; i < current; i++)
            {
                var go = Instantiate(heartPrefab, heartsParent);
                hearts.Add(go.GetComponent<Image>());
            }
        }
        else if (hearts.Count > current)
        {
            for (int i = hearts.Count - 1; i >= current; i--)
            {
                Destroy(hearts[i].gameObject);
                hearts.RemoveAt(i);
            }
        }
    }

    private void AtualizaBossBar()
    {
        if (bossFillImage == null) return;

        float pct = 0f;   // preenchimento 0..1

        // ** Caso boss de torres **
        if (towerSpawner != null)
        {
            // soma a vida atual de cada torre
            int somaAtual = 0;
            foreach (var t in towerSpawner.instancias)
            {
                if (t == null) continue;
                var h = t.GetComponent<Health>();
                if (h != null)
                    somaAtual += (int)currentHealthField.GetValue(h);
            }
            // percentagem relativa à bossMaxHealth
            pct = (float)somaAtual / bossMaxHealth;
        }
        // ** Caso boss “normal” usando Health.cs **
        else if (bossHealth != null)
        {
            int atual = (int)currentHealthField.GetValue(bossHealth);
            pct = (float)atual / bossHealth.maxHealth;
        }

        bossFillImage.fillAmount = Mathf.Clamp01(pct);
    }
}
