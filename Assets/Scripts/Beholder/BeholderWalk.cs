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
    public Vector2 meteorAreaMin = new Vector2(-8f, -4f);
    public Vector2 meteorAreaMax = new Vector2(8f, 4f);

    private Animator animator;
    private Vector2 movement = Vector2.zero;

    private GameObject laser;
    private GameObject leftBeam;
    private GameObject upBeam;
    private float currentAngle = -90f;
    private bool isLaserActive = false;
    private bool isWalking = true;

    void Start()
    {
        animator = GetComponent<Animator>();
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

        if (Input.GetKeyDown(KeyCode.M) && !isLaserActive)
        {
            ShootLaserArc();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            ShootBeams();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(SpawnMeteorRain());
        }

        if (isLaserActive)
        {
            RotateLaser();
        }
    }

    void ShootLaserArc()
    {
        isWalking = false;
        laser = Instantiate(laserPrefab, firePoint.position, Quaternion.Euler(0f, 0f, currentAngle));
        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.up * laserSpeed;
        }

        isLaserActive = true;
        Destroy(laser, 2f);
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
    }

    void ShootBeams()
    {
        isWalking = false;

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

        Invoke("ResumeWalking", 3f);
    }

    void ResumeWalking()
    {
        isWalking = true;
    }

    IEnumerator SpawnMeteorRain()
    {
        isWalking = false;
        Vector3[] positions = new Vector3[meteorCount];
        GameObject[] shadows = new GameObject[meteorCount];

        for (int i = 0; i < meteorCount; i++)
        {
            float x = Random.Range(meteorAreaMin.x, meteorAreaMax.x);
            float y = Random.Range(meteorAreaMin.y, meteorAreaMax.y);
            Vector3 pos = new Vector3(x, y, 0f);
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

        yield return new WaitForSeconds(2f);
        isWalking = true;
    }

    IEnumerator MoveMeteorDown(GameObject meteor, Vector3 target, GameObject shadow)
    {
        float speed = 10f;

        while (Vector3.Distance(meteor.transform.position, target) > 0.1f)
        {
            meteor.transform.position = Vector3.MoveTowards(meteor.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        Destroy(shadow);
        Destroy(meteor, 1f);
    }
}
