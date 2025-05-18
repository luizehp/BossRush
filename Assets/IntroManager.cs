using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    public GameObject beholder;      // Referência ao Beholder
    public GameObject player;        // Referência ao Player
    public Camera mainCamera;        // Referência à câmera principal (filha do player)

    public float cameraSpeed = 2f;   // Velocidade do movimento da câmera
    public float waitBeforeStart = 1f;          // Espera antes da animação começar
    public float beholderIntroDuration = 3f;    // Duração da animação de intro do Beholder

    private BeholderWalk beholderScript;
    private Transform originalCameraParent;

    private const float fixedCameraZ = -4f;     // Posição Z fixa da câmera

    void Start()
    {
        beholderScript = beholder.GetComponent<BeholderWalk>();

        // Desativa o comportamento do Beholder e o player para começar parado
        beholderScript.enabled = false;
        player.SetActive(false);

        // Guarda o pai original da câmera e a desanexa para poder mover livremente
        originalCameraParent = mainCamera.transform.parent;
        mainCamera.transform.parent = null;

        // Começa a sequência de introdução
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        // Move a câmera até o Beholder
        yield return StartCoroutine(MoveCameraTo(beholder.transform.position));

        yield return new WaitForSeconds(waitBeforeStart);

        // Espera a animação de introdução do Beholder (começa automaticamente via Animator)
        yield return new WaitForSeconds(beholderIntroDuration);

        // Move a câmera de volta para o player
        yield return StartCoroutine(MoveCameraTo(player.transform.position));

        // Reanexa a câmera ao player
        mainCamera.transform.parent = originalCameraParent;
        mainCamera.transform.localPosition = new Vector3(0f, 0f, fixedCameraZ);
        mainCamera.transform.localRotation = Quaternion.identity;

        // Ativa o Beholder e o player para começar a luta
        beholderScript.enabled = true;
        beholderScript.StartCombat(); // ✅ Inicia os ataques após a intro
        player.SetActive(true);
    }

    IEnumerator MoveCameraTo(Vector3 targetPosition)
    {
        Vector3 startPos = mainCamera.transform.position;
        targetPosition.z = fixedCameraZ;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * cameraSpeed;
            Vector3 newPos = Vector3.Lerp(startPos, targetPosition, t);
            newPos.z = fixedCameraZ;
            mainCamera.transform.position = newPos;
            yield return null;
        }
    }
}
