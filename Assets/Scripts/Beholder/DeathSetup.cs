using UnityEngine;

public class DeathSetup : MonoBehaviour
{
    public GameObject portaPrefab;
    public GameObject heartPrefab;
    void Awake()
    {
        Animator animator = GetComponent<Animator>();
        foreach (var behaviour in animator.GetBehaviours<Death>())
        {

            behaviour.portaPrefab = portaPrefab;
            behaviour.heartPrefab = heartPrefab;
        }
    }
}
