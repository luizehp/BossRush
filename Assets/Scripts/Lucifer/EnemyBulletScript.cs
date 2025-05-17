using UnityEngine;
using System.Collections;

public class EnemyBulletScript : MonoBehaviour
{
    [Tooltip("Base seconds to hover before launching")]
    public float hoverTime = 1f;

    [Tooltip("Extra seconds on top of hoverTime")]
    [HideInInspector]
    public float launchDelay = 0f;

    public float speed = 10f;

    private Rigidbody2D rb;
    private Transform playerTransform;
    public SpriteRenderer bulletRenderer;
    public AudioSource audioSource;
    public AudioClip launchSound;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb.linearVelocity = Vector2.zero;
        transform.rotation = Quaternion.Euler(0f, 0f, -90);
    }

    void Start()
    {
        StartCoroutine(HoverAndLaunch());
    }

    private IEnumerator HoverAndLaunch()
    {
        yield return new WaitForSeconds(hoverTime + launchDelay);

        audioSource.clip = launchSound;
        audioSource.Play();

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        Destroy(gameObject, 3f);
    }


    public void SetColor(Color color)
    {
        if (bulletRenderer != null)
            bulletRenderer.color = color;
    }

}
