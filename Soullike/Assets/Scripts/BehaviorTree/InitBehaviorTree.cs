using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBehaviorTree
{
    INode _rootNode;

    public InitBehaviorTree(INode rootNode)
    {
        _rootNode = rootNode;
    }

    public void StartBehaviorTree()
    {
        _rootNode.Evaulate();
    }
}
