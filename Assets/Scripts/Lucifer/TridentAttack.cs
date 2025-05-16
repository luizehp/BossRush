using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TridentAttack : MonoBehaviour
{
    public GameObject shadowPrefab;
    public GameObject attackPrefab;
    public float warningTime = 1.5f;
    public float interval = 2f; // time between attacks

    public Transform player;
    private Coroutine attackLoop;
    private List<GameObject> activeShadows = new List<GameObject>();
    private List<GameObject> activeAttacks = new List<GameObject>();

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
        GameObject shadow = Instantiate(shadowPrefab, position, Quaternion.identity);
        activeShadows.Add(shadow);

        yield return new WaitForSeconds(warningTime);

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
