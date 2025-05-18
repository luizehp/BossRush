using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerMenu : MonoBehaviour
{
    [Header("Popup de Controles")]
    public GameObject controlsPopup;

    [Header("Popup de Configurações")]
    public GameObject settingsPopup;

    [Header("Configurações de Cena")]
    public string startSceneName = "GameScene";

    [Header("Configurações")]
    public Slider volumeSlider;

    [Header("UI Lights")]
    public GameObject lightIniciar;
    public GameObject lightControles;
    public GameObject lightConfig;
    public GameObject lightClose;

    private const string VOLUME_KEY = "GlobalVolume";

    void Start()
    {
        // popups começam escondidos
        if (controlsPopup  != null) controlsPopup.SetActive(false);
        if (settingsPopup  != null) settingsPopup.SetActive(false);

        // Carregar volume salvo (default = 1f)
        float savedVol = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
        AudioListener.volume = savedVol;

        // Inicializar e vincular o slider
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = savedVol;
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // luzes começam apagadas
        if (lightIniciar   != null) lightIniciar.SetActive(false);
        if (lightControles != null) lightControles.SetActive(false);
        if (lightConfig    != null) lightConfig.SetActive(false);
        if (lightClose     != null) lightClose.SetActive(false);
    }

    // Iniciar o jogo
    public void PlayGame()
    {
        if (!string.IsNullOrEmpty(startSceneName))
            SceneManager.LoadSceneAsync(startSceneName);
        else
            Debug.LogError("ManagerMenu: defina startSceneName no Inspector!");
    }

    // Mostrar / ocultar popups
    public void ShowControls() => controlsPopup?.SetActive(true);
    public void HideControls() => controlsPopup?.SetActive(false);

    public void ShowSettings() => settingsPopup?.SetActive(true);
    public void HideSettings() => settingsPopup?.SetActive(false);

    // Ajusta o volume global e salva a configuração
    public void SetVolume(float vol)
    {
        AudioListener.volume = vol;
        PlayerPrefs.SetFloat(VOLUME_KEY, vol);
        PlayerPrefs.Save();
    }

    // Hover nos Botões
    public void HoverIniciar_Enter()   => lightIniciar?.SetActive(true);
    public void HoverIniciar_Exit()    => lightIniciar?.SetActive(false);

    public void HoverControles_Enter() => lightControles?.SetActive(true);
    public void HoverControles_Exit()  => lightControles?.SetActive(false);

    public void HoverConfig_Enter()    => lightConfig?.SetActive(true);
    public void HoverConfig_Exit()     => lightConfig?.SetActive(false);

    public void HoverClose_Enter()     => lightClose?.SetActive(true);
    public void HoverClose_Exit()      => lightClose?.SetActive(false);
}
