using UnityEngine;
using System.Collections;

public class TridentAttack : MonoBehaviour
{
    public GameObject shadowPrefab;
    public GameObject attackPrefab;
    public float warningTime = 1.5f;
    public float interval = 2f; // time between attacks

    public Transform player;

    void Start()
    {
        StartCoroutine(AttackLoop());
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

        yield return new WaitForSeconds(warningTime);

        GameObject attack = Instantiate(attackPrefab, position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(attack);
        shadow.GetComponent<Fader>().FadeOut(0.5f);

    }

}
