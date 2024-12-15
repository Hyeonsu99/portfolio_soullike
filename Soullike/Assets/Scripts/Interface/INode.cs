using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    public enum NodeState
    {
        Running,
        Success,
        Failure
    }

    public NodeState Evaulate();
}
