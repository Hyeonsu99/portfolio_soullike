using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float _detectionRange = 25;
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
                            new ActionNode(CheckEnemyInDetectionRange),
                            new ParallelNode
                            (
                                new List<INode>()
                                {
                                    new SequenceNode
                                    (
                                        new List<INode>()
                                        {
                                            new ActionNode(CheckEnemyInAttackRange),
                                            new ActionNode(Attack)
                                        }
                                    ),

                                    new ActionNode(Chase)
                                }
                            )
                        }
                    ),

                }
            );
    }

    INode.NodeState CheckEnemyInDetectionRange()
    {
        var targets = Physics.OverlapSphere(transform.position, _detectionRange, layerMask);

        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        if (targets.Length > 0 && targets != null)
        {
            _target = targets[0].transform;

            return INode.NodeState.Success;
        }

        return INode.NodeState.Failure;
    }

    INode.NodeState CheckEnemyInAttackRange()
    {
        var targets = Physics.OverlapSphere(transform.position, _attackRange, layerMask);

        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        if (targets.Length > 0 && targets != null)
        {
            _target = targets[0].transform;

            return INode.NodeState.Success;
        }

        return INode.NodeState.Failure;
    }

    INode.NodeState Attack()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        return INode.NodeState.Success;
    }

    INode.NodeState Chase()
    {
        Debug.Log("추적 중..");

        if (Vector3.Distance(transform.position, _target.position) < _attackRange)
        {
            Debug.Log("추적 종료");

            return INode.NodeState.Success;
        }


        transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));

        transform.Translate(Vector3.forward * 1f * Time.deltaTime);

        return INode.NodeState.Running;
    }

    INode.NodeState Action1()
    {
        Debug.Log("다른 액션도 실행 중");

        return INode.NodeState.Success;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, _attackRange);

    }
}
