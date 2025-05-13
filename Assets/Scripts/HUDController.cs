using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    [Header("Pause UI")]
    public GameObject buttonPause;
    public GameObject pauseLight;
    public GameObject pausePanel;
    public GameObject pauseCloseLight;
    public GameObject controlsPauseLight;
    public GameObject configPauseLight;
    public GameObject exitPauseLight;

    [Header("Popups")]
    public GameObject controlsPopup;
    public GameObject controlsCloseLight;
    public GameObject settingsPopup;
    public GameObject settingsCloseLight;

    [Header("Cenas")]
    public string mainMenuSceneName = "MainMenu";

    private PlayerHealth playerHealth;
    private List<Image> hearts = new List<Image>();

    private FieldInfo currentHealthField;
    private TowerSpawner towerSpawner;
    private Health bossHealth;
    private int maxTotalHealth;
    private bool maxHealthInitialized = false;

    private bool isPaused = false;

    void Awake()
    {
        // pega PlayerHealth
        playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            Debug.LogError("HUDController: PlayerHealth não encontrado no player!");

        // reflection para currentHealth
        currentHealthField = typeof(Health)
            .GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance);

        // tenta pegar TowerSpawner ou Health do boss
        towerSpawner = boss.GetComponent<TowerSpawner>();
        bossHealth   = boss.GetComponent<Health>();
    }

    void Start()
    {
        // garante que, ao entrar na cena, não fique pausado
        Time.timeScale = 1f;

        // instancia corações iniciais
        for (int i = 0; i < playerHealth.health; i++)
        {
            var go = Instantiate(heartPrefab, heartsParent);
            hearts.Add(go.GetComponent<Image>());
        }

        // barra do boss cheia
        if (bossFillImage != null)
            bossFillImage.fillAmount = 1f;

        // UI de pausa e popups desligados
        pauseLight?.SetActive(false);
        pausePanel?.SetActive(false);
        pauseCloseLight?.SetActive(false);
        controlsPauseLight?.SetActive(false);
        configPauseLight?.SetActive(false);
        exitPauseLight?.SetActive(false);
        controlsPopup?.SetActive(false);
        controlsCloseLight?.SetActive(false);
        settingsPopup?.SetActive(false);
        settingsCloseLight?.SetActive(false);
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

    // alterna o estado de pausa
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel?.SetActive(isPaused);
    }

    // popups
    public void ShowControls()  => controlsPopup?.SetActive(true);
    public void HideControls()  => controlsPopup?.SetActive(false);
    public void ShowSettings()  => settingsPopup?.SetActive(true);
    public void HideSettings()  => settingsPopup?.SetActive(false);

    // volta ao menu principal e garante unpause
    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Hover effects
    public void HoverPause_Enter()             => pauseLight?.SetActive(true);
    public void HoverPause_Exit()              => pauseLight?.SetActive(false);

    public void HoverPauseClose_Enter()        => pauseCloseLight?.SetActive(true);
    public void HoverPauseClose_Exit()         => pauseCloseLight?.SetActive(false);

    public void HoverControlsPause_Enter()     => controlsPauseLight?.SetActive(true);
    public void HoverControlsPause_Exit()      => controlsPauseLight?.SetActive(false);

    public void HoverConfigPause_Enter()       => configPauseLight?.SetActive(true);
    public void HoverConfigPause_Exit()        => configPauseLight?.SetActive(false);

    public void HoverExitPause_Enter()         => exitPauseLight?.SetActive(true);
    public void HoverExitPause_Exit()          => exitPauseLight?.SetActive(false);

    public void HoverControlsClose_Enter()     => controlsCloseLight?.SetActive(true);
    public void HoverControlsClose_Exit()      => controlsCloseLight?.SetActive(false);

    public void HoverSettingsClose_Enter()     => settingsCloseLight?.SetActive(true);
    public void HoverSettingsClose_Exit()      => settingsCloseLight?.SetActive(false);
}
