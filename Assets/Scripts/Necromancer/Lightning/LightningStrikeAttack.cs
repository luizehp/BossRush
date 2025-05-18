using System;
using System.Collections;
using Necromancer.Tower;
using UnityEngine;
using UnityEngine.Rendering.Universal; // Importante para usar Light2D

namespace Necromancer.Lightning
{
    public class LightningStrikeAttack : MonoBehaviour
    {
        public AudioSource thunderAttackClip;

        private enum State { Idle, CastingShadow, LockingPosition, Striking, Recovering }
        private State currentState = State.Idle;

        public Animator animator;
        public Animator lightningAnimator;
        public GameObject shadowPrefab;
        public GameObject lightningPrefab;
        public float shadowFollowDuration = 1.5f;

        private GameObject player;
        private GameObject shadow;
        private GameObject lightning;
        private Vector3 lockedPosition;

        private float timer;
        private bool wasCalled = false;

        public Shake shakeScript;
        public TowerSpawner towerSpawnerScript;

        private Light2D shadowLight;
        private float blinkTimer = 0f;

        void Start()
        {
            currentState = State.Idle;
            player = GameObject.FindWithTag("Player");
        }

        void Update()
        {
            if (currentState == State.Idle)
            {
                if (!wasCalled) return;
                currentState = State.CastingShadow;
                timer = 0f;

                shadow = Instantiate(shadowPrefab, player.transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
                shadowLight = shadow.GetComponentInChildren<Light2D>();
            }

            else if (currentState == State.CastingShadow)
            {
                timer += Time.deltaTime;

                if (shadow && player)
                    shadow.transform.position = player.transform.position - new Vector3(0, 0.5f, 0);

                // Piscar mais rápido quanto mais perto do raio
                float blinkFrequency = Mathf.Lerp(1.0f, 0.05f, timer / shadowFollowDuration);
                blinkTimer += Time.deltaTime;

                if (blinkTimer >= blinkFrequency)
                {
                    blinkTimer = 0f;
                    if (shadowLight != null)
                    {
                        Debug.Log("Blinking Light2D");
                        shadowLight.intensity = shadowLight.intensity > 0.5f ? 0f : 1f;
                    }
                }

                if (timer < shadowFollowDuration) return;

                currentState = State.LockingPosition;
                timer = 0f;

                if (shadow != null)
                    lockedPosition = shadow.transform.position;
            }

            else if (currentState == State.LockingPosition)
            {
                currentState = State.Striking;

                if (shadow != null)
                    Destroy(shadow);

                lightning = Instantiate(lightningPrefab, lockedPosition, Quaternion.identity);
                lightningAnimator.SetTrigger("LightBolt");

                if (thunderAttackClip != null)
                    thunderAttackClip.Play();
            }

            else if (currentState == State.Striking)
            {
                currentState = State.Recovering;
                StartCoroutine(RecoverAfterStrike(0.5f));
            }
        }

        public void ExecuteLightningStrike()
        {
            wasCalled = true;
        }

        IEnumerator RecoverAfterStrike(float duration)
        {
            yield return new WaitForSeconds(duration);
            currentState = State.Idle;
            wasCalled = false;
        }

        public void TriggerShake()
        {
            if (towerSpawnerScript.Ended) return;
            shakeScript.TriggerShake();
        }
    }
}
