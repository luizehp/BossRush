using UnityEngine;
using System.Collections;

public class Death : StateMachineBehaviour
{
    public GameObject portaPrefab;
    public GameObject heartPrefab;

    private Transform cameraTransform;
    private Transform playerTransform;
    private Vector3 offset;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject boss = animator.gameObject;
        playerTransform = GameObject.FindWithTag("Player").transform;
        cameraTransform = Camera.main.transform;

        // Salva a distância (offset) entre a câmera e o player
        offset = cameraTransform.position - playerTransform.position;

        // Desvincula a câmera temporariamente
        cameraTransform.SetParent(null);

        // Move a câmera para o boss
        Vector3 bossCameraPosition = boss.transform.position + offset;
        bossCameraPosition.z = cameraTransform.position.z;
        cameraTransform.position = bossCameraPosition;

        // Instancia a porta em posição fixa
        if (portaPrefab != null)
        {
            Vector3 posicaoFixa = new Vector3(-13.2990913f, 8.50909042f, 0f);
            Debug.Log("Posição fixa da porta: " + posicaoFixa);
            GameObject porta = GameObject.Instantiate(portaPrefab, posicaoFixa, Quaternion.identity);
        }

        // Instancia o coração em posição fixa
        if (heartPrefab != null)
        {
            Vector3 posicaoFixa2 = new Vector3(-14.8400002f, 5.5999999f, 0f);
            GameObject.Instantiate(heartPrefab, posicaoFixa2, Quaternion.identity);
        }

        // Usa a CÂMERA como runner para evitar ser destruído
        CoroutineRunner runner = cameraTransform.gameObject.GetComponent<CoroutineRunner>();
        if (runner == null)
        {
            runner = cameraTransform.gameObject.AddComponent<CoroutineRunner>();
        }

        runner.StartCoroutine(HandleDeathSequence(boss, 2f));
    }

    private IEnumerator HandleDeathSequence(GameObject boss, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Destroi o boss
        GameObject.Destroy(boss);

        yield return new WaitForSeconds(0.5f);

        if (playerTransform != null && cameraTransform != null)
        {
            Vector3 newCameraPosition = playerTransform.position + offset;
            newCameraPosition.z = cameraTransform.position.z;
            cameraTransform.position = newCameraPosition;

            // Reanexa a câmera ao player
            cameraTransform.SetParent(playerTransform);
        }
    }

    // Componente auxiliar para rodar corrotinas fora de um MonoBehaviour direto
    private class CoroutineRunner : MonoBehaviour { }
}
