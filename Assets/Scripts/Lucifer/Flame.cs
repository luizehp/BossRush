using UnityEngine;

public class Flame : MonoBehaviour
{
    public ParticleSystem flame1;
    public ParticleSystem flame2;

    public GameObject light1;
    public GameObject light2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            light1.SetActive(true);
            light2.SetActive(true);
            var emission1 = flame1.emission;
            emission1.enabled = true;
            var emission2 = flame2.emission;
            emission2.enabled = true;
        }
    }
}