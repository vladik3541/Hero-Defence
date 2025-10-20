using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    public event  Action<GameObject> OnCurrentTarget;

    [SerializeField] private float _speed;

    [SerializeField] private float _distanceForAttack;


    private List<GameObject> _targets = new List<GameObject>();

    private GameObject _currentTarget;

    private GameObject _targeDefault;

    private Animator animator;
    private Quaternion initialRotation;

    private NavMeshAgent navMeshAgent;

    private void OnEnable()
    {
        UnitHealth.OnDeadUnit += RemoveTarget;
    }
    private void OnDisable()
    {
        UnitHealth.OnDeadUnit -= RemoveTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        _targeDefault = GameObject.FindGameObjectWithTag("Castle");
        _targets.Add(_targeDefault);
        _currentTarget = _targets[0];

        initialRotation = transform.rotation;

        navMeshAgent.speed = _speed;

    }

    // Update is called once per frame
    void Update()
    {
        SwapTarget();
        DistansForAttack();
    }

    private void SwapTarget()
    {
        if(_targets.Count > 1)
        {
            if (_targets[1] != null)
            {
                _currentTarget = _targets[1];
            }
            else
            {
                _targets.RemoveAt(1);
                _currentTarget = _targets[0];
            }
        } 
        if(_targets.Count == 1)
        {
            _currentTarget = _targets[0];
        }
        
    }
    
    private void DistansForAttack()
    {
        float distans = Vector3.Distance(transform.position, _currentTarget.transform.position);
        if(_distanceForAttack >= distans)
        {
            navMeshAgent.isStopped = true;

            navMeshAgent.velocity = Vector3.zero; // для моментальної зупинки

            OnCurrentTarget?.Invoke(_currentTarget);

            animator.SetBool("Attack", true);

            
            navMeshAgent.speed = 0;
            LookToTarget();
        }
        else if (distans >= _distanceForAttack + 1)
        {
            animator.SetBool("Attack", false);
            navMeshAgent.speed = _speed;

            navMeshAgent.isStopped = false;

            navMeshAgent.SetDestination(_currentTarget.transform.position);
        }
    }
    void LookToTarget()
    {
        Vector3 directionToTarget = _currentTarget.transform.position - transform.position;
        directionToTarget.y = 0;

        transform.rotation = Quaternion.LookRotation(directionToTarget.normalized) * initialRotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out UnitHealth unitHealth))
        {
            _targets.Add(unitHealth.gameObject);
        }
    }

    private void RemoveTarget(GameObject target)
    {
        _targets.Remove(target);
    }
}
