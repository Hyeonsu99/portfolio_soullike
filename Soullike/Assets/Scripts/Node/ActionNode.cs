using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : INode
{
    public delegate INode.NodeState Action();
    Action action;

    public ActionNode(Action action)
    {
        this.action = action;
    }

    public INode.NodeState Evaulate()
    {
        if(action == null)
        {
            return INode.NodeState.Failure;
        }

        return action.Invoke();
    }
}
