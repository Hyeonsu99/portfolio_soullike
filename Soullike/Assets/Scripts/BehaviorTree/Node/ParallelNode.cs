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
        bool _isAnyNodeRunning = false;
        bool _isAllNodeSuccess = false;

        if (_child == null || _child.Count == 0)
        {
            return INode.NodeState.Failure;
        }

        foreach(var node in _child)
        {
            var state = node.Evaulate();

            if(state == INode.NodeState.Running)
            {
                _isAnyNodeRunning = true;
            }
            else if(state == INode.NodeState.Failure)
            {
                _isAllNodeSuccess = false;
            }
        }

        if(_isAllNodeSuccess)
        {
            return INode.NodeState.Success;
        }
        if(_isAnyNodeRunning)
        {
            return INode.NodeState.Running;
        }

        return INode.NodeState.Failure;
    }
}
