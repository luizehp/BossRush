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

        //public AudioSource multiFireAudio;
        //public AudioSource singleFireballAudio;

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
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            Vector3 spawnPos = multiFireSpawn.position;
            spawnPos.z = 0f;
            //singleFireballAudio.Play();
            GameObject fireball = Instantiate(singleFireballPrefab, spawnPos, rotation);
    
            Animator fireballAnimator = fireball.GetComponent<Animator>();
            if (fireballAnimator is not null)
            {
                fireballAnimator.SetTrigger("SingleCall");
            }

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
                //multiFireAudio.Play();
                GameObject burst = Instantiate(burstFireballPrefab, spawnPos, rotation);
                Animator fireballAnimator = burst.GetComponent<Animator>();
                if (fireballAnimator is not null)
                {
                    fireballAnimator.SetTrigger("BurstCall");
                }
            }

            yield return null;
        }
    }
}