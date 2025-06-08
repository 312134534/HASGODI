using UnityEngine;

[CreateNodeMenu("AI/Action")]
public class AIActionNode : AINode
{
    public enum ActionType { MoveRight, MoveLeft, Jump, Dodge, Attack, RangedAttack }
    public ActionType action;

    [Input] public AINode input;
    [Output] public AINode next;

    public override void Execute(AIBrain brain)
    {
        if (brain == null || brain.Input == null)
        {
            Debug.LogWarning("AIActionNode: brain 或 brain.Input 為 null");
            return;
        }

        switch (action)
        {
            case ActionType.MoveRight: brain.Input.SetMove(Vector2.right); break;
            case ActionType.MoveLeft: brain.Input.SetMove(Vector2.left); break;
            case ActionType.Jump: brain.Input.Jump(); break;
            case ActionType.Dodge: brain.Input.Dodge(); break;
            case ActionType.Attack: brain.Input.CloseCombat(); break;
            case ActionType.RangedAttack: brain.Input.RangedAttack(); break;
        }

        var port = GetOutputPort("next");
        if (port != null && port.Connection != null)
        {
            AINode nextNode = port.Connection.node as AINode;
            if (nextNode != null)
            {
                nextNode.Execute(brain);
            }
        }
        else
        {
            Debug.LogWarning("AIActionNode 的 next port 沒有接線");
        }
    }
}
