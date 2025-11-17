using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    public event Action<GameObject> OnCurrentTarget;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float distanceForAttack = 1.5f;
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private LayerMask targetMask;

    private GameObject defaultTarget;
    private GameObject currentTarget;

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Quaternion initialRotation;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        defaultTarget = GameObject.FindGameObjectWithTag("Castle");
        currentTarget = defaultTarget;

        initialRotation = transform.rotation;
        navMeshAgent.speed = speed;
    }

    void Update()
    {
        UpdateTarget();
        UpdateAttackState();
    }

    private void UpdateTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            float d = Vector3.Distance(transform.position, hit.transform.position);
            if (d < minDist)
            {
                minDist = d;
                nearest = hit.gameObject;
            }
        }

        currentTarget = nearest != null ? nearest : defaultTarget;
    }

    private void UpdateAttackState()
    {
        if (currentTarget == null) return;

        float dist = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (dist <= distanceForAttack)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;

            animator.SetBool("Attack", true);
            OnCurrentTarget?.Invoke(currentTarget);

            LookToTarget();
        }
        else
        {
            animator.SetBool("Attack", false);
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = speed;
            navMeshAgent.SetDestination(currentTarget.transform.position);
        }
    }

    private void LookToTarget()
    {
        Vector3 dir = currentTarget.transform.position - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(dir.normalized) * initialRotation;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
