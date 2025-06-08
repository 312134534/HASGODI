using UnityEngine;
using XNode;

public abstract class AINode : Node
{
    public abstract void Execute(AIBrain brain);
}