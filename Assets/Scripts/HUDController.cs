using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Necromancer.Tower;

public class HUDController : MonoBehaviour
{
    [Header("Referências - Player")]
    public GameObject player;
    public GameObject heartPrefab;
    public Transform heartsParent;

    [Header("Boss Health Bar")]
    public GameObject boss;
    public Image bossFillImage;

    private PlayerHealth playerHealth;
    private List<Image> hearts = new List<Image>();

    private FieldInfo currentHealthField;
    private TowerSpawner towerSpawner;
    private Health bossHealth;
    private int maxTotalHealth;
    private bool maxHealthInitialized = false;

    void Awake()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            Debug.LogError("HUDController: não encontrou PlayerHealth no player!");

        currentHealthField = typeof(Health)
            .GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance);

        towerSpawner = boss.GetComponent<TowerSpawner>();
        bossHealth   = boss.GetComponent<Health>();
    }

    void Start()
    {
        for (int i = 0; i < playerHealth.health; i++)
        {
            var go = Instantiate(heartPrefab, heartsParent);
            hearts.Add(go.GetComponent<Image>());
        }
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

        if (!maxHealthInitialized)
        {
            if (towerSpawner != null && towerSpawner.instancias.Count > 0)
            {
                maxTotalHealth = towerSpawner.instancias
                    .Where(t => t != null && t.GetComponent<Health>() != null)
                    .Sum(t => t.GetComponent<Health>().maxHealth);
                maxHealthInitialized = true;
            }
            else if (bossHealth != null)
            {
                maxTotalHealth = bossHealth.maxHealth;
                maxHealthInitialized = true;
            }
        }

        int current = 0;
        if (towerSpawner != null && maxHealthInitialized)
        {
            foreach (var t in towerSpawner.instancias)
            {
                if (t == null) continue;
                var h = t.GetComponent<Health>();
                if (h != null)
                    current += (int)currentHealthField.GetValue(h);
            }
        }
        else if (bossHealth != null && maxHealthInitialized)
        {
            current = (int)currentHealthField.GetValue(bossHealth);
        }

        float pct = maxTotalHealth > 0
            ? (float)current / maxTotalHealth
            : 0f;

        bossFillImage.fillAmount = Mathf.Clamp01(pct);
    }
}
