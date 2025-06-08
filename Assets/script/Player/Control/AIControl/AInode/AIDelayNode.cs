using UnityEngine;

[CreateNodeMenu("AI/Delay")]
public class AIDelayNode : AINode
{
    public float delaySeconds = 1f;

    [Input] public AINode input;
    [Output] public AINode next;

    public override void Execute(AIBrain brain)
    {
        brain.StartCoroutine(DelayAndContinue(brain));
    }

    private System.Collections.IEnumerator DelayAndContinue(AIBrain brain)
    {
        yield return new WaitForSeconds(delaySeconds);

        AINode nextNode = GetOutputPort("next").Connection?.node as AINode;
        if (nextNode != null)
        {
            nextNode.Execute(brain);
        }
    }
}
