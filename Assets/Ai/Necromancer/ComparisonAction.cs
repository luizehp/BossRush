using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Comparison", story: "[Seed] > [Percentage] and <= [Percentage_2]", category: "Action", id: "8d2e8dbd085b22e6c5abe4381807fa73")]
public partial class ComparisonAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Seed;
    [SerializeReference] public BlackboardVariable<float> Percentage;
    [SerializeReference] public BlackboardVariable<float> Percentage_2;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Debug.Log("Seed: " + Seed.Value + ", Percentage: " + Percentage.Value + ", Percentage2: " + Percentage_2.Value);
        if ((Seed > Percentage) && (Seed <= Percentage_2))
            return Status.Success;
        return Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}

