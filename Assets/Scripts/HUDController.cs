using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
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
    public Image    bossFillImage;

    [Header("Endgame UI")]
    public GameObject endGamePanelVictory;
    public GameObject endGamePanelDefeat;
    public bool      eh_finalBoss;

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

    private PlayerHealth    playerHealth;
    private List<Image>     hearts = new List<Image>();

    private FieldInfo       currentHealthField;
    private TowerSpawner    towerSpawner;
    private Health          bossHealth;
    private BossController  bossController;
    private int             maxTotalHealth;
    private bool            maxHealthInitialized = false;

    private bool defeatTriggered  = false;
    private bool victoryTriggered = false;
    private bool isPaused         = false;

    void Awake()
    {
        // Pega PlayerHealth
        playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            Debug.LogError("HUDController: PlayerHealth não encontrado no player!");

        // Reflection para currentHealth de Health
        currentHealthField = typeof(Health)
            .GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance);

        // Tenta pegar TowerSpawner, Health ou BossController do boss
        towerSpawner   = boss.GetComponent<TowerSpawner>();
        bossHealth     = boss.GetComponent<Health>();
        bossController = boss.GetComponent<BossController>();
    }

    void Start()
    {
        // Garante que, ao entrar na cena, não fique pausado
        Time.timeScale = 1f;

        // Instancia corações iniciais
        for (int i = 0; i < playerHealth.health; i++)
        {
            var go = Instantiate(heartPrefab, heartsParent);
            hearts.Add(go.GetComponent<Image>());
        }

        // Barra do boss cheia
        if (bossFillImage != null)
            bossFillImage.fillAmount = 1f;

        // Desliga UI de pausa e popups
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

        // Esconde telas de endgame
        endGamePanelVictory?.SetActive(false);
        endGamePanelDefeat?.SetActive(false);
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

        // Inicializa maxTotalHealth apenas uma vez
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
            else if (bossController != null)
            {
                maxTotalHealth = bossController.maxHealth;
                maxHealthInitialized = true;
            }
        }

        // Obtém vida atual
        int current = 0;
        if (towerSpawner != null && maxHealthInitialized)
        {
            current = towerSpawner.instancias
                .Where(t => t != null && t.GetComponent<Health>() != null)
                .Sum(t => (int)currentHealthField.GetValue(t.GetComponent<Health>()));
        }
        else if (bossHealth != null && maxHealthInitialized)
        {
            current = (int)currentHealthField.GetValue(bossHealth);
        }
        else if (bossController != null && maxHealthInitialized)
        {
            current = bossController.currentHealth;
        }

        // Atualiza barra
        bossFillImage.fillAmount = Mathf.Clamp01(
            maxTotalHealth > 0 ? (float)current / maxTotalHealth : 0f
        );

        // Verifica condições de fim de jogo
        CheckEndGame();
    }

    private void CheckEndGame()
    {
        // Derrota: delay de 2s
        if (!defeatTriggered && playerHealth.health <= 0)
        {
            defeatTriggered = true;
            StartCoroutine(ShowDefeatAfterDelay());
        }

        // Vitória (só se final boss): delay de 5s
        if (!victoryTriggered && eh_finalBoss)
        {
            bool victory = false;

            if (towerSpawner != null && maxHealthInitialized)
            {
                int sumHealth = towerSpawner.instancias
                    .Where(t => t != null && t.GetComponent<Health>() != null)
                    .Sum(t => (int)currentHealthField.GetValue(t.GetComponent<Health>()));
                victory = sumHealth <= 0;
            }
            else if (bossHealth != null)
            {
                victory = (int)currentHealthField.GetValue(bossHealth) <= 0;
            }
            else if (bossController != null)
            {
                victory = bossController.currentHealth <= 0;
            }

            if (victory)
            {
                victoryTriggered = true;
                StartCoroutine(ShowVictoryAfterDelay());
            }
        }
    }

    private IEnumerator ShowDefeatAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
        endGamePanelDefeat.SetActive(true);
    }
    
    private IEnumerator ShowVictoryAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        Time.timeScale = 0f;
        endGamePanelVictory.SetActive(true);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel?.SetActive(isPaused);
    }

    public void ShowControls()  => controlsPopup?.SetActive(true);
    public void HideControls()  => controlsPopup?.SetActive(false);
    public void ShowSettings()  => settingsPopup?.SetActive(true);
    public void HideSettings()  => settingsPopup?.SetActive(false);

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

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
