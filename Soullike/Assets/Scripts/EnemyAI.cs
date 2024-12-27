using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private float _detectionRange = 30;
    private float _attackRange = 25;
    private float _moveSpeed = 2f;

    private NavMeshAgent _agent;

    public Transform _target;

    public LayerMask layerMask;

    InitBehaviorTree _initBehaviorTree;

    public float _bombPatternTime = 5f;
    public float _currentbombPatternTime = 0f;

    public bool _isRush = false;
    public bool _isBomb = false;

    public float _rushPatternTime = 10f;
    public float _currentRushPatternTime = 0f;

    private float _rushDistance = 30f;
    public float _remainDistance;

    public Transform[] bombPoints;

    private void Awake()
    {
        _initBehaviorTree = new InitBehaviorTree(SettingBT());

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _moveSpeed;
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

                            new ParallelNode
                            (
                                new List<INode>()
                                {
                                    new SequenceNode
                                    (
                                        new List<INode>()
                                        {
                                            new ActionNode(CheckEnemyInAttackRange),
                                            new ActionNode(CheckRushPattern),
                                            new ActionNode(CheckBombPattern),
                                        }
                                    ),

                                    new ActionNode(Chase)
                                }
                            )
                        }                      
                    ),

                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(Action1)
                        }
                    )
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

    INode.NodeState Chase()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        if (_target == null)
        {
            return INode.NodeState.Failure;
        }
        else
        {
            if(!_isRush)
            {
                transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));

                _agent.destination = _target.position;
            }

            return INode.NodeState.Success;
        }
    }

    INode.NodeState CheckBombPattern()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        if(!_isRush)
        {
            _currentbombPatternTime += Time.deltaTime;

            if (_currentbombPatternTime >= _bombPatternTime)
            {
                _currentbombPatternTime = 0;

                StartCoroutine(BombCoroutine());

                return INode.NodeState.Success;
            }
        }

        return INode.NodeState.Failure;
    }

    private IEnumerator BombCoroutine()
    {
        _currentbombPatternTime = 0;

        Debug.Log("폭탄 패턴 시작");

        _agent.speed = 0f;

        for (int i = 0; i < bombPoints.Length; i++)
        {
            var obj = ObjectPool.GetObject(ObjectPool.instance.bombObjectQueue, ObjectPool.instance.bombObjectPrefab);

            obj.transform.position = bombPoints[i].position;

            var rigidBody = obj.GetComponent<Rigidbody>();

            if (rigidBody != null)
            {
                Vector3 force = bombPoints[i].transform.forward * 5f;

                rigidBody.AddForce(force, ForceMode.Impulse);
            }
        }

        yield return new WaitForSeconds(1f);

        _agent.speed = _moveSpeed;

        yield return null;
    }

    INode.NodeState CheckRushPattern()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);

        _remainDistance = _agent.remainingDistance;

        if (_agent.remainingDistance < _rushDistance)
        {
            if (!_isRush)
            {
                _currentRushPatternTime += Time.deltaTime;
            }

            if (_currentRushPatternTime >= _rushPatternTime)
            {
                StartCoroutine(RushCoroutine());
            }
        }

        return INode.NodeState.Success;
    }

    private IEnumerator RushCoroutine()
    {
        Debug.Log("돌진 패턴 시작");

        _currentRushPatternTime = 0f;

        _isRush = true;

        Vector3 lastPlayerPosition = FindAnyObjectByType<PlayerController>().transform.position;

        _agent.speed = _moveSpeed * 5;

        transform.LookAt(lastPlayerPosition);
        transform.Translate(Vector3.forward * _agent.speed * Time.deltaTime);

        yield return new WaitUntil(() => _remainDistance <= 1f);

        yield return new WaitForSeconds(1f);

        Debug.Log("돌진 패턴 끝");

        _agent.speed = _moveSpeed;
        _agent.SetDestination(_target.position);

        _isRush = false;

        yield return null;
    }

    private IEnumerator GatlingCoroutine()
    {
        Debug.Log("개틀링 코루틴 시작");

        yield return new WaitForSeconds(1f);

        Debug.Log("개틀링 코루틴 끝");
    }

    INode.NodeState Action1()
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
