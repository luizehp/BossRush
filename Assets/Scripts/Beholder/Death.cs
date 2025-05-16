using UnityEngine;
using System.Collections;

public class Death : StateMachineBehaviour
{
    [HideInInspector] public GameObject portaPrefab;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Instancia a porta em uma posição fixa
        if (portaPrefab != null)
        {
            Vector3 posicaoFixa = new Vector3(-47.7f, 12.05497f, -1f);
            GameObject.Instantiate(portaPrefab, posicaoFixa, Quaternion.identity);
        }

        // Inicia uma corrotina para destruir o Beholder depois de 2 segundos
        CoroutineRunner runner = animator.gameObject.AddComponent<CoroutineRunner>();
        runner.StartCoroutine(DestroyAfterDelay(animator.gameObject, 2f));
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(obj);
    }

    // Classe temporária para permitir execução de corrotinas fora de MonoBehaviour
    private class CoroutineRunner : MonoBehaviour { }
}
