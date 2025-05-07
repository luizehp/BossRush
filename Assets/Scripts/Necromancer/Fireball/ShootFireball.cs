using System.Collections;
using UnityEngine;

namespace Necromancer.Fireball
{
    public class ShootFireball : MonoBehaviour
    {
        public GameObject singleFireballPrefab;
        public GameObject burstFireballPrefab;
        public Transform fireBurstSpawn;
        public Transform multiFireSpawn;
        public Animator animator;
        public int intervalo = 20;
        public Transform player;

        [Header("Áudio")]
        public AudioClip singleFireSFX;
        public AudioClip burstFireSFX;
        public float audioVolume = 1f;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
                animator.SetTrigger("FireballBurst");
            if (Input.GetKeyDown(KeyCode.N))
                animator.SetTrigger("MultiFire");
        }

        public void ShootFireballArcEvent()
        {
            StartCoroutine(ShootFireballArc());
        }

        public void ShootOneFireball()
        {
            Vector3 spawnPos = multiFireSpawn.position;
            Vector3 direction = player.position - spawnPos;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            print(angle);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            GameObject fireball = Instantiate(singleFireballPrefab, spawnPos, rotation);
    
            Animator fireballAnimator = fireball.GetComponent<Animator>();
            if (fireballAnimator != null)
                fireballAnimator.SetTrigger("SingleCall");
    
            if (singleFireSFX != null)
                AudioSource.PlayClipAtPoint(singleFireSFX, spawnPos, audioVolume);
        }

        private IEnumerator ShootFireballArc()
        {
            int initAngle = -90;
            int finalAngle = 90;
            for (int ang = initAngle; ang <= finalAngle; ang += intervalo)
            {
                Quaternion rotation = Quaternion.Euler(0, 0, ang);
                Vector3 spawnPos = fireBurstSpawn.position;
                spawnPos.z = 0f;
                GameObject burst = Instantiate(burstFireballPrefab, spawnPos, rotation);
                Animator fireballAnimator = burst.GetComponent<Animator>();
                if (fireballAnimator != null)
                    fireballAnimator.SetTrigger("BurstCall");
                if (burstFireSFX != null)
                    AudioSource.PlayClipAtPoint(burstFireSFX, spawnPos, audioVolume);
            }
            yield return null;
        }
    }
}
