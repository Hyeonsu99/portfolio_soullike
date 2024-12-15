using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : INode
{
    List<INode> _child;

    public SequenceNode(List<INode> child)
    {
        _child = child;
    }

    public INode.NodeState Evaulate()
    {
        if(_child.Count == 0 || _child == null)
        {
            return INode.NodeState.Failure;
        }

        foreach (var node in _child)
        {
            switch (node.Evaulate())
            {
                case INode.NodeState.Running:
                    return INode.NodeState.Running;
                case INode.NodeState.Success:
                    continue;
                case INode.NodeState.Failure:
                    return INode.NodeState.Failure;            
            }
        }

        return INode.NodeState.Success;
    }
}
