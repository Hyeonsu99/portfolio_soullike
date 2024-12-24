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

    private float _bombPatternTime = 0.0f;
    private float _rushPatternTime = 0.0f;

    public Transform[] bombPoints;

    private void Awake()
    {
        _initBehaviorTree = new InitBehaviorTree(SettingBT());

        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _initBehaviorTree.StartBehaviorTree();          
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

    INode.NodeState CheckBombPattern()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        _bombPatternTime += Time.deltaTime;

        if(_bombPatternTime >= 30f)
        {
            _bombPatternTime = 0;

            StartCoroutine(BombCoroutine());

            return INode.NodeState.Running;
        }

        return INode.NodeState.Failure;
    }

    INode.NodeState CheckRushPattern()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        _rushPatternTime += Time.deltaTime;

        if(_rushPatternTime >= 3f)
        {
            _rushPatternTime = 0;

            StartCoroutine(RushCoroutine());

            return INode.NodeState.Running;
        }

        return INode.NodeState.Failure;
    }

    private IEnumerator BombCoroutine()
    {
        for(int i = 0; i < bombPoints.Length; i++)
        {
            var obj = ObjectPool.GetObject(ObjectPool.instance.bombObjectQueue , ObjectPool.instance.bombObjectPrefab);

            obj.transform.position = bombPoints[i].position;

            var rigidBody = obj.GetComponent<Rigidbody>();

            if(rigidBody != null)
            {
                Vector3 force = bombPoints[i].forward * 5f;

                rigidBody.AddForce(force, ForceMode.Impulse);
            }
        }

        yield return null;
    }

    private IEnumerator GatlingCoroutine()
    {
        Debug.Log("개틀링 코루틴 시작");

        yield return new WaitForSeconds(1f);

        Debug.Log("개틀링 코루틴 끝");
    }
    
    private IEnumerator RushCoroutine()
    {
        Vector3 lastPlayerPosition = FindAnyObjectByType<PlayerController>().transform.position;

        _agent.speed = 10f;
        _agent.SetDestination(lastPlayerPosition);

        if(_agent.remainingDistance < 1f)
        {
            _agent.speed = 3.5f;
            _agent.SetDestination(_target.position);

            yield return null;
        }    
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, _attackRange);

    }
}
