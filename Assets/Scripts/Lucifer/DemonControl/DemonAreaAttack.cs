using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemonAreaAttack : MonoBehaviour
{
    public GameObject shadowWarningPrefab;
    public GameObject attackPrefab;
    public int numberOfShadows = 10;
    public float warningTime = 2f;
    public float attackDuration = 1f;
    public Vector2 attackAreaSize = new Vector2(10, 5);
    public float minDistanceBetweenShadows = 1.5f;

    private List<GameObject> activeShadows = new List<GameObject>();
    private List<GameObject> activeAttacks = new List<GameObject>();

    public BossController bossController;
    public float phaseTwoAttackInterval = 8f;
    public float shadowFadeOutDuration = 0.5f;
    public AudioSource audioSource;
    public AudioClip attackSound;


    private void Start()
    {
        enabled = false;
    }
    private void OnEnable()
    {
        if (bossController != null && bossController.IsInPhaseTwo())
        {
            StartCoroutine(PhaseTwoAttackLoop());
        }

    }

    private IEnumerator PhaseTwoAttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(phaseTwoAttackInterval);
            StartShadowAttack();
        }
    }

    public void StartShadowAttack()
    {
        if (bossController != null && bossController.IsInPhaseTwo())
        {
            StartCoroutine(ShadowAttackSequence());
        }
    }



    private IEnumerator ShadowAttackSequence()
    {
        List<Vector2> positions = GenerateShadowPositions();

        foreach (Vector2 pos in positions)
        {
            GameObject shadow = Instantiate(shadowWarningPrefab, pos, Quaternion.identity);
            activeShadows.Add(shadow);
            StartCoroutine(FadeInShadow(shadow));
        }

        yield return new WaitForSeconds(warningTime);

        audioSource.clip = attackSound;
        audioSource.pitch = 1.1f;
        audioSource.volume = 1.5f;
        audioSource.Play();
        foreach (Vector2 pos in positions)
        {
            GameObject attack = Instantiate(attackPrefab, pos, Quaternion.identity);
            activeAttacks.Add(attack);
            StartCoroutine(RemoveAttackAfterDelay(attack, attackDuration));
        }

        yield return new WaitForSeconds(attackDuration);

        foreach (GameObject shadow in activeShadows)
        {
            if (shadow != null)
            {
                StartCoroutine(FadeOutShadow(shadow));
            }
        }
        activeShadows.Clear();
    }

    private IEnumerator FadeInShadow(GameObject shadow)
    {
        SpriteRenderer sr = shadow.GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;
        float fadeDuration = warningTime * 0.8f;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0, 1, t / fadeDuration));
            yield return null;
        }
    }

    private IEnumerator FadeOutShadow(GameObject shadow)
    {
        SpriteRenderer sr = shadow.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        Color originalColor = sr.color;
        float startAlpha = originalColor.a;

        for (float t = 0; t < shadowFadeOutDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(startAlpha, 0, t / shadowFadeOutDuration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(shadow);
    }

    private List<Vector2> GenerateShadowPositions()
    {
        List<Vector2> positions = new List<Vector2>();
        int attempts = 0;

        while (positions.Count < numberOfShadows && attempts < 100)
        {
            Vector2 newPos = (Vector2)transform.position + new Vector2(
                Random.Range(-attackAreaSize.x / 2, attackAreaSize.x / 2),
                Random.Range(-attackAreaSize.y / 2, attackAreaSize.y / 2)
            );

            bool validPosition = true;
            foreach (Vector2 existingPos in positions)
            {
                if (Vector2.Distance(newPos, existingPos) < minDistanceBetweenShadows)
                {
                    validPosition = false;
                    break;
                }
            }

            if (validPosition)
            {
                positions.Add(newPos);
            }
            attempts++;
        }
        return positions;
    }

    private IEnumerator RemoveAttackAfterDelay(GameObject attack, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (attack != null)
        {
            activeAttacks.Remove(attack);
            Destroy(attack);
        }
    }

    public void StopAllAttacks()
    {
        StopAllCoroutines();

        foreach (GameObject shadow in activeShadows)
        {
            if (shadow != null) Destroy(shadow);
        }

        foreach (GameObject attack in activeAttacks)
        {
            if (attack != null) Destroy(attack);
        }

        activeShadows.Clear();
        activeAttacks.Clear();
    }
}