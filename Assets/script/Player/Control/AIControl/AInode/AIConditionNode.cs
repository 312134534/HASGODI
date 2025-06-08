using UnityEngine;

[CreateNodeMenu("AI/Condition")]
public class AIConditionNode : AINode
{
    public enum ConditionType
    {
        DistanceLessThan,
        TargetOnRight,
        TargetOnLeft,
        IsHittingWall,
        FacingRight,
        IsBulletCome,
        IsOnPlayer
    }

    public ConditionType conditionType = ConditionType.DistanceLessThan;
    public float distanceThreshold = 3f;

    [Input] public AINode input;
    [Output] public AINode ifTrue;
    [Output] public AINode ifFalse;

    public override void Execute(AIBrain brain)
    {
        Vector2 toTarget = brain.Target.position - brain.transform.position;
        float dist = toTarget.magnitude;
        bool conditionMet = false;

        switch (conditionType)
        {
            case ConditionType.DistanceLessThan:
                conditionMet = dist < distanceThreshold;
                break;

            case ConditionType.TargetOnRight:
                conditionMet = toTarget.x > 0f;
                break;

            case ConditionType.TargetOnLeft:
                conditionMet = toTarget.x < 0f;
                break;

            case ConditionType.IsHittingWall:
                conditionMet = brain.IsHittingWall;
                break;

            case ConditionType.FacingRight:
                conditionMet = brain.FacingRight;
                break;
            case ConditionType.IsBulletCome:
                conditionMet = brain.DetectIncomingBullets();
                break;
            case ConditionType.IsOnPlayer:
                conditionMet = brain.IsOnPlayer;
                break;
        }

        var port = GetOutputPort(conditionMet ? "ifTrue" : "ifFalse");
        if (port != null && port.Connection != null)
        {
            AINode nextNode = port.Connection.node as AINode;
            if (nextNode != null)
            {
                nextNode.Execute(brain);
            }
        }
    }
}
