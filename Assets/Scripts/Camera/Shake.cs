using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shake : MonoBehaviour
{
    public bool start = false;
    public AnimationCurve curve;
    public float duration = 1f;

    private Vector3 baseOffset = new Vector3(0, 0, -4); // posição fixa da câmera em relação ao player

    public void Update()
    {
        if (start)
        {
            start = false;
            TriggerShake();
        }
    }

    public void TriggerShake()
    {
        StopAllCoroutines();
        StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);

            Vector3 shake = Random.insideUnitSphere * strength;
            shake.z = 0; // mantemos o z fixo

            transform.localPosition = baseOffset + shake;

            yield return null;
        }

        transform.localPosition = baseOffset; // volta exatamente pra posição correta
    }
}
