using UnityEngine;

public class Slash : MonoBehaviour
{
    public GameObject slashPrefab;
    public float extendedRange = 1f;
    public Transform playerTransform;

    public void SlashPre(Vector2 direction)
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.hasSlashAbility)
        {

            Vector2 spawnPosition = (Vector2)transform.position + direction * extendedRange;

            GameObject slash = Instantiate(slashPrefab, spawnPosition, Quaternion.identity);
            slash.transform.SetParent(playerTransform);


            Transform slashT = slash.transform;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            slashT.rotation = Quaternion.Euler(0, 0, angle + 90);

            Destroy(slash, 0.4f);
        }
    }
}
