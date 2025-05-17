using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightTrigger : MonoBehaviour
{
    public Light2D light1;
    public Light2D light2;
    public float targetIntensity = 3f;
    public float duration = 2f;
    public AudioClip sfx; // Som a ser tocado ao acender a luz
    public float volume = 1f;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;

            // Toca o SFX uma vez no mesmo GameObject
            if (sfx != null)
                AudioSource.PlayClipAtPoint(sfx, transform.position, volume);

            // Inicia o fade das luzes
            StartCoroutine(FadeInLight(light1));
            StartCoroutine(FadeInLight(light2));
        }
    }

    private System.Collections.IEnumerator FadeInLight(Light2D light)
    {
        float startIntensity = light.intensity;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
            yield return null;
        }

        light.intensity = targetIntensity;
    }
}
