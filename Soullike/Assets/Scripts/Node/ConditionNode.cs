using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : INode
{
    public delegate bool Condition();
    Condition condition;

    public ConditionNode(Condition condition)
    {
        this.condition = condition;
    }

    public INode.NodeState Evaulate()
    {
        if(condition == null)
        {
            return INode.NodeState.Failure;
        }

        return condition.Invoke() ? INode.NodeState.Success : INode.NodeState.Failure;
    }
}
