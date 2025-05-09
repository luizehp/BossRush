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

    void Start()
    {
        // popups começam escondidos
        if (controlsPopup  != null) controlsPopup.SetActive(false);
        if (settingsPopup  != null) settingsPopup.SetActive(false);
        // slider inicia no volume atual
        if (volumeSlider   != null) volumeSlider.value = AudioListener.volume;
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

    // Controles
    public void ShowControls() => controlsPopup?.SetActive(true);
    public void HideControls() => controlsPopup?.SetActive(false);

    // Configurações
    public void ShowSettings() => settingsPopup?.SetActive(true);
    public void HideSettings() => settingsPopup?.SetActive(false);

    // Volume
    public void SetVolume(float vol) => AudioListener.volume = vol;

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
