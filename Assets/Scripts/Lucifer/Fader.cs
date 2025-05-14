using UnityEngine;

public class Fader : MonoBehaviour
{
    private SpriteRenderer sr;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            Debug.LogError("Fader requires a SpriteRenderer on the same GameObject.");
    }

    public void FadeOut(float duration)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeAndDestroy(duration));
    }

    private System.Collections.IEnumerator FadeAndDestroy(float duration)
    {
        Color originalColor = sr.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
