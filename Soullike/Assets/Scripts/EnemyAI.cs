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
                            new ActionNode(CheckingPlayerInDetectionRange),
                            new ActionNode(Chase),
                            //new ConditionNode(CheckAttackRange),
                            //new ActionNode(Attack)
                        }
                    )
                }
            );
    }

    INode.NodeState CheckingPlayerInDetectionRange()
    {
        var targets = Physics.OverlapSphere(transform.position, _detectionRange, layerMask);

        Debug.Log("추적 중...");

        if (targets != null && targets.Length > 0)
        {
            _target = targets[0].transform;

            Debug.Log("추적 끝...");

            return INode.NodeState.Success;
        }

        _target = null;
        return INode.NodeState.Failure;
    }

    bool CheckDetectionRange()
    {
        var targets = Physics.OverlapSphere(transform.position, _detectionRange, layerMask);

        if (targets != null && targets.Length > 0)
        {
            _target = targets[0].transform;

            return true;
        }

        _target = null;
        return false;
    }

    INode.NodeState Chase()
    {
        if (_target != null)
        {
            if (Vector3.Distance(_target.position, transform.position) > _attackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime);

                return INode.NodeState.Running;
            }
            else
            {
                return INode.NodeState.Success;
            }
        }

        return INode.NodeState.Failure;
    }

    bool CheckAttackRange()
    {
        return false;
    }

    INode.NodeState Attack()
    {
        return INode.NodeState.Success;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, _attackRange);

    }
}
