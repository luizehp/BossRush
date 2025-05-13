using Necromancer;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "OnSpawn", story: "[NecromancerScript] activates [hasSpawned]", category: "Action", id: "43f5418fec6094b8ac18c3b7a8292105")]
public partial class OnSpawnAction : Action
{
    [SerializeReference] public BlackboardVariable<NecromancerSpawn> NecromancerScript;
    [SerializeReference] public BlackboardVariable<bool> HasSpawned;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()    
    {
        Debug.Log(NecromancerScript.Value);
        return Status.Running;
    }

    protected override void OnEnd()
    {
        Debug.Log("Ended");
        HasSpawned.Value = true;
    }
}

