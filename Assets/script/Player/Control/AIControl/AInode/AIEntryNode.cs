using UnityEngine;

[CreateNodeMenu("AI/Entry")]
public class AIEntryNode : AINode
{
    [Output] public AINode next;

    public override void Execute(AIBrain brain)
    {
        AINode nextNode = GetOutputPort("next").Connection.node as AINode;
        if (nextNode != null)
        {
            nextNode.Execute(brain);
        }
    }
}
