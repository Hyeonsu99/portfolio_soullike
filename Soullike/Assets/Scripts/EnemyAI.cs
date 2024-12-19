using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float _detectionRange = 10;
    private float _attackRange = 3;

    public Transform _target;

    public LayerMask layerMask;

    InitBehaviorTree _initBehaviorTree;

    // Start is called before the first frame update
    void Start()
    {
        _initBehaviorTree = new InitBehaviorTree(SettingBT());
    }

    // Update is called once per frame
    void Update()
    {
        _initBehaviorTree.StartBehaviorTree();

        //var targets = Physics.OverlapSphere(transform.position, _detectionRange, layerMask);

        //if (targets != null && targets.Length > 0)
        //{
        //    _target = targets[0].transform;
        //}
            
    }

    INode SettingBT()
    {
        return new SelectorNode
            (
                new List<INode>()
                {
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(Action2),

                            new ParallelNode
                            (
                                new List<INode>()
                                {
                                    new ActionNode(Action3),
                                    new ActionNode(Action1)
                                }
                            )
                        }
                    )
                }
            );
    }

    INode.NodeState Action1()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        return INode.NodeState.Success;
    }

    INode.NodeState Action2()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        return INode.NodeState.Success;
    }
    INode.NodeState Action3()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        return INode.NodeState.Failure;
    }
    INode.NodeState Action4()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        return INode.NodeState.Running;
    }
    INode.NodeState Action5()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        return INode.NodeState.Success;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, _attackRange);

    }
}
