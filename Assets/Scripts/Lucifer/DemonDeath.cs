using UnityEngine;
using System.Collections;


public class DemonDeath : MonoBehaviour
{
    public Camera mainCamera;
    public Transform playerTransform;
    public GameObject Trident;
    public float cameraTransitionTime = 1f;
    public float deathAnimationDuration = 2f;
    public PlayerMovement playerMovement;
    public TridentAttack tridentAttack;
    public DemonAreaAttack demonAreaAttack;

    public void Die()
    {
        if (playerMovement != null)
            playerMovement.enabled = false;
        if (tridentAttack != null)
        {
            tridentAttack.StopAllCoroutines();
            tridentAttack.StopAllAttacks();
        }
        if (demonAreaAttack != null)
        {
            demonAreaAttack.StopAllAttacks();
            demonAreaAttack.enabled = false;
        }
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
        // Disable AI and attack components
        GetComponent<DemonAI>().enabled = false;
        GetComponent<EnemyJumpAttack>().enabled = false;
        GetComponent<DemonFB>().enabled = false;

        // Stop any movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        StartCoroutine(CameraTransition());
    }

    private IEnumerator CameraTransition()
    {
        // Store original camera parent and position
        Transform originalCameraParent = mainCamera.transform.parent;
        Vector3 originalCameraLocalPos = mainCamera.transform.localPosition;
        Vector3 originalCameraPos = mainCamera.transform.position;
        var playerRb = playerTransform.GetComponent<Rigidbody2D>();
        var trail = playerTransform.GetComponent<TrailRenderer>();

        playerMovement.enabled = false;
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.simulated = false;
        }

        if (trail != null)
        {
            trail.emitting = false;
            trail.Clear();
        }

        mainCamera.transform.parent = null;

        Vector3 targetPosition = transform.position;
        targetPosition.z = originalCameraPos.z;

        float elapsedTime = 0f;
        while (elapsedTime < cameraTransitionTime)
        {
            mainCamera.transform.position = Vector3.Lerp(originalCameraPos, targetPosition, elapsedTime / cameraTransitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for death animation
        yield return new WaitForSeconds(deathAnimationDuration);

        // Calculate return position (player position with original Z)
        Vector3 returnPosition = playerTransform.position;
        returnPosition.z = originalCameraPos.z;

        // Move camera back to player
        elapsedTime = 0f;
        while (elapsedTime < cameraTransitionTime)
        {
            mainCamera.transform.position = Vector3.Lerp(targetPosition, returnPosition, elapsedTime / cameraTransitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reparent and reset camera position
        mainCamera.transform.parent = originalCameraParent;
        mainCamera.transform.localPosition = originalCameraLocalPos;
        if (playerMovement != null)
        {
            if (playerRb != null) playerRb.simulated = true;
            playerMovement.enabled = true;
        }

    }
}