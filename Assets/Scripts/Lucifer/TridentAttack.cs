using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;


public class TridentAttack : MonoBehaviour
{
    public GameObject shadowPrefab;
    public GameObject attackPrefab;
    public float warningTime = 1.5f;
    public float interval = 2f;
    public BossController bossController;

    public Transform player;
    private Coroutine attackLoop;
    private List<GameObject> activeShadows = new List<GameObject>();
    private List<GameObject> activeAttacks = new List<GameObject>();
    public AudioSource audioSource;
    public AudioClip attackSound;

    void Start()
    {
        StartAttacks();
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {

            float x = Random.Range(-1.5f, 1.5f);
            float y = Random.Range(-1.5f, 1.5f);
            Vector3 randomOffset = new Vector3(x, y);
            StartCoroutine(AttackSequence(player.position + randomOffset));
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator AttackSequence(Vector2 position)
    {
        if (bossController != null && bossController.phaseTwo)
        {
            attackPrefab.GetComponent<SpriteRenderer>().color = Color.cyan;
            shadowPrefab.GetComponent<SpriteRenderer>().color = Color.cyan;
            Light2D attackLight = shadowPrefab.GetComponent<Light2D>() ?? shadowPrefab.GetComponentInChildren<Light2D>();
            if (attackLight != null)
            {
                attackLight.color = Color.cyan;
            }

        }
        else
        {
            attackPrefab.GetComponent<SpriteRenderer>().color = Color.white;
            shadowPrefab.GetComponent<SpriteRenderer>().color = Color.red;
            Light2D attackLight = shadowPrefab.GetComponent<Light2D>() ?? shadowPrefab.GetComponentInChildren<Light2D>();
            if (attackLight != null)
            {
                attackLight.color = Color.red;
            }
        }
        GameObject shadow = Instantiate(shadowPrefab, position, Quaternion.identity);
        activeShadows.Add(shadow);

        yield return new WaitForSeconds(warningTime);

        audioSource.clip = attackSound;
        audioSource.Play();
        GameObject attack = Instantiate(attackPrefab, position, Quaternion.identity);
        activeAttacks.Add(attack);


        yield return new WaitForSeconds(1f);

        Destroy(attack);
        shadow.GetComponent<Fader>().FadeOut(0.5f);
    }

    public void StartAttacks()
    {
        if (attackLoop == null)
        {
            attackLoop = StartCoroutine(AttackLoop());
        }
    }
    public void StopAllAttacks()
    {
        if (attackLoop != null)
        {
            StopCoroutine(attackLoop);
            attackLoop = null;
        }
        StopAllCoroutines();

        activeShadows.RemoveAll(item => item == null);
        activeAttacks.RemoveAll(item => item == null);

        foreach (var shadow in activeShadows)
        {
            if (shadow != null)
            {
                Destroy(shadow);
            }
        }

        foreach (var attack in activeAttacks)
        {
            if (attack != null)
            {
                Destroy(attack);
            }
        }

        activeShadows.Clear();
        activeAttacks.Clear();
    }
}
