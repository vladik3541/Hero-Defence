using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class UnitControl : MonoBehaviour
{
    public event Action<GameObject> OnAttack;

    [SerializeField] private float _radiusAttack;
    [SerializeField] private float _distanceAttack;
    [SerializeField, Min(0f)] private float _speedAttack;
    private GameObject _positionForAttackLimit;

    private List<GameObject> _targets;


    bool _isAttacking;
    bool _attackingTarget;

    Rigidbody rb;
    Animator animator;
    NavMeshAgent navMeshAgent;
    private Quaternion initialRotation;
    
    void Start()
    {
        _positionForAttackLimit = GameObject.FindGameObjectWithTag("Spawn");

        initialRotation = transform.rotation;
        GetComponent<SphereCollider>().radius = _radiusAttack;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        _targets = new List<GameObject>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovingAttack();
        StopActions(); 

        if (!_targets.Any()) return;
        if (_targets[0] == null)//���� 1 ������� null �������� �� ������
        {
            _targets.Remove(_targets[0]);
            return;
        }
        LookToTarget();
        MoveToTaget();
        
    }

    private void MovingAttack() // ��� � ������� ������ 
    {
        if (Input.GetKey(KeyCode.F) && !_targets.Any() && !_attackingTarget) // ������ ��� �� ������ ������ 
        {
            _isAttacking = true;
        }
        
        if(_isAttacking)
        {
            animator.SetBool("Run", true);

            navMeshAgent.SetDestination(_positionForAttackLimit.transform.position);
            Debug.Log("Run");
        }
        if (_targets.Any())
        {
            _isAttacking = false;
        }
    }
    private void MoveToTaget()
    {
        
        float distance = Vector3.Distance(transform.position, _targets[0].transform.position);

        if(distance <= _distanceAttack)
        {
            //Attack
            _attackingTarget = true;

            if(_attackingTarget)
            {
                navMeshAgent.isStopped = true;
                navMeshAgent.velocity = Vector3.zero;


                OnAttack?.Invoke(_targets[0]);
                animator.SetBool("Run", false);
                animator.SetBool("Attack", true);
                animator.speed = _speedAttack;
            }
            return;

        }
        else if(distance >= _distanceAttack+1)
        {
            //DontStop
            navMeshAgent.isStopped = false;

            navMeshAgent.SetDestination(_targets[0].transform.position);
            Debug.Log("RunTarget");
            animator.SetBool("Run", true);
            animator.SetBool("Attack", false);
        }

    }
    private void LookToTarget()
    {
        Vector3 direction = _targets[0].transform.position - transform.position;

        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction.normalized) * initialRotation;
    }
    private void StopActions()
    {
        if(_attackingTarget)
        {
            animator.SetBool("Run", false);
        }
        if(!_targets.Any())
        {
            animator.speed = 1;
            animator.SetBool("Attack", false);
            _attackingTarget = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            _targets.Add(enemyHealth.gameObject);
        }
    }
    private void RemoveTarget(GameObject enemy)
    {
        _targets.Remove(enemy);
    }
}
