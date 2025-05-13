using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Necromancer.PreRoom
{
    public class LightsUp : MonoBehaviour
    {
        public Light2D light2D;
        public float targetIntensity = 1.5f;
        public float duration = 1f;

        void Start()
        {
            light2D.intensity = 0f;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(FadeInLight());
            }
        }
        
        private IEnumerator FadeInLight()
        {
            float startIntensity = light2D.intensity;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                light2D.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
                yield return null;
            }

            light2D.intensity = targetIntensity;
        }
    }
}