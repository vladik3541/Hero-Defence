using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitControl : MonoBehaviour
{
    public event Action<GameObject> OnAttack;

    [SerializeField] private float radiusAttack = 6f;
    [SerializeField] private float distanceAttack = 1.7f;
    [SerializeField] private float speedAttack = 1f;
    [SerializeField] private LayerMask enemyMask;

    private GameObject attackLimitPoint;

    private bool isManualAttack;
    private bool isAttackingTarget;

    private Animator animator;
    private NavMeshAgent agent;
    private Quaternion initialRotation;

    void Start()
    {
        attackLimitPoint = GameObject.FindGameObjectWithTag("Spawn");

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        initialRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        HandleManualMove();
        GameObject target = FindNearestEnemy();

        if (target == null)
        {
            ResetCombatState();
            return;
        }

        MoveOrAttack(target);
        LookTo(target);
    }
    private GameObject FindNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radiusAttack, enemyMask);

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

        return nearest;
    }
    
    private void HandleManualMove()
    {
        if (Input.GetKey(KeyCode.F))
        {
            isManualAttack = true;
        }

        if (isManualAttack)
        {
            animator.SetBool("Run", true);
            agent.isStopped = false;
            agent.SetDestination(attackLimitPoint.transform.position);
        }
    }
    
    private void MoveOrAttack(GameObject target)
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);

        isManualAttack = false;

        if (dist <= distanceAttack)
        {
            isAttackingTarget = true;

            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            animator.SetBool("Run", false);
            animator.SetBool("Attack", true);
            animator.speed = speedAttack;

            OnAttack?.Invoke(target);
        }
        else
        {
            isAttackingTarget = false;

            agent.isStopped = false;
            agent.SetDestination(target.transform.position);

            animator.SetBool("Run", true);
            animator.SetBool("Attack", false);
            animator.speed = 1f;
        }
    }
    
    private void LookTo(GameObject target)
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(dir.normalized) * initialRotation;
        }
    }
    
    private void ResetCombatState()
    {
        isAttackingTarget = false;

        animator.SetBool("Attack", false);
        animator.speed = 1f;

        if (!isManualAttack)
        {
            animator.SetBool("Run", false);
            agent.isStopped = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
    }
}
