using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "condition", story: "If [hasSpawned]", category: "Flow/Conditional", id: "ea5728abebde0c8ff3a9c3db7f75d8dc")]
public partial class ConditionAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> HasSpawned;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

