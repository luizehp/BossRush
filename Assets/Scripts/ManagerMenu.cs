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

    void Start()
    {
        if (controlsPopup != null)
            controlsPopup.SetActive(false);

        if (settingsPopup != null)
            settingsPopup.SetActive(false);

        if (volumeSlider != null)
            volumeSlider.value = AudioListener.volume;
    }

    // chamado pelo botão “Iniciar”
    public void PlayGame()
    {
        if (!string.IsNullOrEmpty(startSceneName))
            SceneManager.LoadSceneAsync(startSceneName);
        else
            Debug.LogError("ManagerMenu: defina startSceneName no Inspector!");
    }

    // chamado pelo botão “Controles”
    public void ShowControls()
    {
        if (controlsPopup != null)
            controlsPopup.SetActive(true);
    }

    // chamado pelo “X” no popup de controles
    public void HideControls()
    {
        if (controlsPopup != null)
            controlsPopup.SetActive(false);
    }

    // chamado pelo botão “Configurações”
    public void ShowSettings()
    {
        if (settingsPopup != null)
            settingsPopup.SetActive(true);
    }

    // chamado pelo “Fechar” no popup de configurações
    public void HideSettings()
    {
        if (settingsPopup != null)
            settingsPopup.SetActive(false);
    }

    // vinculado ao Slider → On Value Changed
    public void SetVolume(float vol)
    {
        AudioListener.volume = vol;
    }
}
