using UnityEngine;
using System.Collections;

public class BeholderWalk : MonoBehaviour
{
    public GameObject target;
    public float speed = 2f;
    public float desiredDistance = 3f;
    public float tolerance = 0.1f;

    public GameObject laserPrefab;
    public GameObject beamPrefab;
    public GameObject shadowPrefab;
    public GameObject meteorPrefab;
    public Transform firePoint;
    public float laserSpeed = 10f;

    public int meteorCount = 4;

    private Animator animator;
    private Vector2 movement = Vector2.zero;

    private GameObject laser;

    private GameObject secondLaser;
    private GameObject leftBeam;
    private GameObject rightBeam;
    private GameObject downBeam;
    private GameObject upBeam;
    private float currentAngle = -90f;
    private bool isLaserActive = false;
    private bool isWalking = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AttackRoutine());
    }

    void Update()
    {
        if (isWalking)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

            movement = Vector2.zero;

            if (distanceToTarget < desiredDistance - tolerance)
            {
                movement = -direction;
                transform.position += (Vector3)(movement * speed * Time.deltaTime);
            }
            else if (distanceToTarget > desiredDistance + tolerance)
            {
                movement = direction;
                transform.position += (Vector3)(movement * speed * Time.deltaTime);
            }

            if (movement.x != 0 || movement.y != 0)
            {
                animator.SetFloat("X", movement.x);
                animator.SetFloat("Y", movement.y);
                animator.SetBool("IsWalk", true);
            }
            else
            {
                animator.SetBool("IsWalk", false);
            }
        }
        else
        {
            animator.SetBool("IsWalk", false);
        }

        if (isLaserActive)
        {
            RotateLaser();
        }
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // Tempo antes de atacar

            int attackChoice = Random.Range(1, 4); // 1, 2 ou 3

            switch (attackChoice)
            {
                case 1:
                    if (!isLaserActive) yield return StartCoroutine(ShootLaserArc());
                    break;

                case 2:
                    if (!isLaserActive) yield return StartCoroutine(ShootBeams());
                    break;
                case 3:
                    yield return StartCoroutine(SpawnMeteorRain());
                    break;
            }

            yield return new WaitForSeconds(3f); // Tempo andando após o ataque
        }
    }

    IEnumerator ShootLaserArc()
    {
        isWalking = false;
        animator.SetBool("Laser", true); // Inicia a animação de preparo

        yield return new WaitForSeconds(1.7f); // Espera antes de atirar

        // Instancia os lasers
        laser = Instantiate(laserPrefab, firePoint.position, Quaternion.Euler(0f, 0f, currentAngle));
        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.up * laserSpeed;
        }

        secondLaser = Instantiate(laserPrefab, firePoint.position, Quaternion.Euler(0f, 0f, currentAngle + 270f));
        Rigidbody2D rb2 = secondLaser.GetComponent<Rigidbody2D>();
        if (rb2 != null)
        {
            rb2.linearVelocity = Vector2.up * laserSpeed;
        }

        isLaserActive = true;
        Destroy(laser, 2f);
        Destroy(secondLaser, 2f);

        // Finaliza a animação depois que os lasers forem destruídos
        StartCoroutine(StopLaserAnimationAfterDelay(1.5f));
    }
    IEnumerator StopLaserAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("Laser", false);
    }



    void RotateLaser()
    {
        currentAngle += 90f * Time.deltaTime;

        if (currentAngle >= 90f)
        {
            currentAngle = -90f;
            isLaserActive = false;
            isWalking = true;
        }

        if (laser != null)
        {
            laser.transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);

        }
        if (secondLaser != null)
        {
            secondLaser.transform.rotation = Quaternion.Euler(0f, 0f, currentAngle + 270f);
        }
    }

    IEnumerator ShootBeams()
    {
        isWalking = false;
        animator.SetBool("Laser2", true); // Inicia a animação de preparo
        Debug.Log("Preparing to shoot beams...");
        
        yield return new WaitForSeconds(1.4f); // Espera antes de atirar

        if (leftBeam == null)
        {
            leftBeam = Instantiate(beamPrefab, firePoint.position, Quaternion.Euler(0f, 0f, 270f));
            Destroy(leftBeam, 3f);
        }

        if (upBeam == null)
        {
            upBeam = Instantiate(beamPrefab, firePoint.position, Quaternion.Euler(0f, 0f, 0f));
            Destroy(upBeam, 3f);
        }

        if (rightBeam == null)
        {
            rightBeam = Instantiate(beamPrefab, firePoint.position, Quaternion.Euler(0f, 0f, 90f));
            Destroy(rightBeam, 3f);
        }

        if (downBeam == null)
        {
            downBeam = Instantiate(beamPrefab, firePoint.position, Quaternion.Euler(0f, 0f, 180f));
            Destroy(downBeam, 3f);
        }

        // Finaliza a animação depois que os lasers forem destruídos
        yield return new WaitForSeconds(2.25f);
        animator.SetBool("Laser2", false);
        Invoke("ResumeWalking", 3f);
    }

    void ResumeWalking()
    {
        isWalking = true;
    }

    IEnumerator SpawnMeteorRain()
    {
        isWalking = false;
        animator.SetBool("Meteor", true); // Inicia a animação de preparo
        yield return new WaitForSeconds(1.5f); // Espera antes de atirar

        float areaWidth = 10f;
        float areaHeight =10f;

        Vector3[] positions = new Vector3[meteorCount];
        GameObject[] shadows = new GameObject[meteorCount];

        float minDistance = 3f;

        for (int i = 0; i < meteorCount; i++)
        {
            Vector3 pos;
            do
            {
                float x = Random.Range(target.transform.position.x - areaWidth / 2f, target.transform.position.x + areaWidth / 2f);
                float y = Random.Range(target.transform.position.y - areaHeight / 2f, target.transform.position.y + areaHeight / 2f);
                pos = new Vector3(x, y, 0f);
            }
            while (Mathf.Abs(pos.x - target.transform.position.x) < minDistance &&
                Mathf.Abs(pos.y - target.transform.position.y) < minDistance);

            positions[i] = pos;
            shadows[i] = Instantiate(shadowPrefab, pos, Quaternion.identity);
        }


        yield return new WaitForSeconds(1f);

        for (int i = 0; i < meteorCount; i++)
        {
            Vector3 spawnPos = positions[i] + new Vector3(0f, 5f, 0f);
            GameObject meteor = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
            StartCoroutine(MoveMeteorDown(meteor, positions[i], shadows[i]));
        }

        // Finaliza a animação depois que os lasers forem destruídos
        yield return new WaitForSeconds(2.25f);
        animator.SetBool("Meteor", false);
        yield return new WaitForSeconds(2f);
        isWalking = true;
    }

    IEnumerator MoveMeteorDown(GameObject meteor, Vector3 target, GameObject shadow)
    {
        float speed = 10f;

        // Desativa o Collider2D no início
        Collider2D col = meteor.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        while (Vector3.Distance(meteor.transform.position, target) > 0.1f)
        {
            meteor.transform.position = Vector3.MoveTowards(meteor.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        // Ativa o Collider2D ao chegar
        if (col != null) col.enabled = true;

        Destroy(shadow);
        Destroy(meteor, 1f);
    }

    
}
