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

        // Instancia a porta
        if (portaPrefab != null)
        {
            Vector3 posicaoFixa = playerTransform.position + new Vector3(5f, 2f, 0);
            Debug.Log("Posição da porta: " + posicaoFixa);

            GameObject porta = GameObject.Instantiate(portaPrefab, posicaoFixa, Quaternion.identity);

            // Ajuste fino, caso o pivot do prefab esteja incorreto (ajuste se necessário)
            porta.transform.position += new Vector3(0f, 0f, 0f); // Altere se precisar centralizar melhor
        }

        // Instancia o coração
        if (heartPrefab != null)
        {
            Vector3 posicaoFixa2 = playerTransform.position + new Vector3(5, 0, 0);
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
