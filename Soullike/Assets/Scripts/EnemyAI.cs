using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private float _detectionRange = 25;
    private float _attackRange = 3;
    private float _moveSpeed = 2f;

    private NavMeshAgent _agent;

    public Transform _target;

    public LayerMask layerMask;

    InitBehaviorTree _initBehaviorTree;

    public float _bombPatternTime = 5f;
    public float _currentbombPatternTime = 5f;

    public bool _isRush = false;

    public float _rushPatternTime = 10f;

    public float _currentRushPatternTime = 10f;

    private float _rushDistance = 20f;
    public float _remainDistance;

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
                            new ActionNode(CheckRushPattern),
                        }                      
                    ),

                    
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            //new ActionNode(CheckEnemyInAttackRange),
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

            return INode.NodeState.Success;
        }
        else
        {
            if(!_isRush)
            {
                _agent.speed = _moveSpeed;
            }
        }

        if(_target == null)
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


        _currentbombPatternTime += Time.deltaTime;

        if(_currentbombPatternTime >= _bombPatternTime)
        {
            _currentbombPatternTime = 0;

            StartCoroutine(BombCoroutine());

            return INode.NodeState.Running;
        }

        return INode.NodeState.Failure;
    }

    private IEnumerator BombCoroutine()
    {
        for (int i = 0; i < bombPoints.Length; i++)
        {
            var obj = ObjectPool.GetObject(ObjectPool.instance.bombObjectQueue, ObjectPool.instance.bombObjectPrefab);

            obj.transform.position = bombPoints[i].position;

            var rigidBody = obj.GetComponent<Rigidbody>();

            if (rigidBody != null)
            {
                Vector3 force = bombPoints[i].forward * 5f;

                rigidBody.AddForce(force, ForceMode.Impulse);
            }
        }

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
        _agent.SetDestination(lastPlayerPosition);

        
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
    




    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, _attackRange);

    }
}
