using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : INode
{
    List<INode> _child;

    public SelectorNode(List<INode> child)
    {
        _child = child;
    }

    public INode.NodeState Evaulate()
    {
        if(_child == null || _child.Count == 0)
        {
            return INode.NodeState.Failure;
        }

        foreach(var node in _child)
        {
            switch(node.Evaulate())
            {
                case INode.NodeState.Running:
                    return INode.NodeState.Running;
                case INode.NodeState.Success:
                    return INode.NodeState.Success;
            }
        }

        return INode.NodeState.Failure;
    }
}
