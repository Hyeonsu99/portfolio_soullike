using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private float _detectionRange = 25;
    private float _attackRange = 3;

    private NavMeshAgent _agent;

    public Transform _target;

    public LayerMask layerMask;

    InitBehaviorTree _initBehaviorTree;

    private float _patternTime = 0.0f;

    private void Awake()
    {
        _initBehaviorTree = new InitBehaviorTree(SettingBT());

        _agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
                            new ActionNode(Chase),
                            new SequenceNode
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

                                    new ActionNode(CheckPatternTime)
                                }


                            )
                        }
                    ),

                }
            ); ;
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
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        if (Vector3.Distance(transform.position, _target.position) <= _attackRange)
        {
            _agent.speed = 0f;

            Debug.Log("추적 종료");

            return INode.NodeState.Success;
        }

        if(_target == null)
        {
            return INode.NodeState.Failure;
        }

        transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));

        _agent.destination = _target.position;
        _agent.speed = 2f;

        return INode.NodeState.Running;
    }

    INode.NodeState CheckPatternTime()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        _patternTime += Time.deltaTime;

        if(_patternTime >= 3f)
        {
            _patternTime = 0;

            StartCoroutine(PatternCoroutine());

            return INode.NodeState.Success;
        }

        return INode.NodeState.Failure;
    }

    private IEnumerator PatternCoroutine()
    {
        Debug.Log("패턴 코루틴 시작");

        yield return new WaitForSeconds(1f);

        Debug.Log("패턴 코루틴 끝");
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, _attackRange);

    }
}
