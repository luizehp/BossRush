using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    public string sceneToLoad = "NomeDaCena";
    public float delayBeforeLoad = 1.5f;
    private CanvasGroup fadeCanvas;

    private bool triggered;

    private void Start()
    {
        fadeCanvas = FindFirstObjectByType<CanvasGroup>();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player"))
            return;

        triggered = true;
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        fadeCanvas.gameObject.SetActive(true);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = t;
            yield return null;
        }

        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}