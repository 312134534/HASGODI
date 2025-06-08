using UnityEngine;

[CreateNodeMenu("AI/Random")]
public class AIRandomNode : AINode
{
    [Range(0, 1)]
    public float trueProbability = 0.5f;

    [Input] public AINode input;
    [Output] public AINode ifTrue;
    [Output] public AINode ifFalse;

    public override void Execute(AIBrain brain)
    {
        bool result = Random.value < trueProbability;
        string selectedPort = result ? "ifTrue" : "ifFalse";

        AINode nextNode = GetOutputPort(selectedPort).Connection?.node as AINode;
        if (nextNode != null)
        {
            nextNode.Execute(brain);
        }
    }
}
