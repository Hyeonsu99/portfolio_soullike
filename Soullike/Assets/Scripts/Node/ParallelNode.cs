using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 항상 모든 노드를 실행함.
public class ParallelNode : INode
{
    List<INode> _child;

    public ParallelNode(List<INode> child)
    {
        _child = child;
    }

    public INode.NodeState Evaulate()
    {
        if (_child == null || _child.Count == 0)
        {
            return INode.NodeState.Failure;
        }

        foreach(var node in _child)
        {
            node.Evaulate();
        }

        return INode.NodeState.Running;
    }
}
