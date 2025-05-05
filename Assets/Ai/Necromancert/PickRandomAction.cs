using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Pick Random", story: "Pick a random [number] from 0 to 1", category: "Action", id: "7b8f9974b8558a2e94ebeabe4870d5e7")]
public partial class PickRandomAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Number;
    
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Number.Value = UnityEngine.Random.Range(0.3f, 1f);
        Debug.Log($"Seed sorteada: {Number.Value}");
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

